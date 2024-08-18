using Spear.Data;
using Spear.States.General;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Spear.States
{
    public class UmbrellaState : SpearState
    {
        private bool _startHoldingExpand;

        private float _holdShrinkTime;
        private float _holdExpandTime;

        private SpearStateSettings Settings => SpearData.SpearConfig.UmbrellaSettings;

        public UmbrellaState(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher,
            spearData, spear)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (SpearData.TipPoint.IsLocked) SpearData.TipPoint.UnLock();
            _startHoldingExpand = SpearData.WasExpandRequest;
            SpearData.loadTimer = 0f;
        }

        public override void HandleRotation()
        {
            base.HandleRotation();
            Quaternion desiredRotQ = Quaternion.Euler(0, 0, SpearData.desiredRotation);
            Quaternion smoothedRotation = Quaternion.Lerp(SpearData.CenterTransform.rotation, desiredRotQ,
                Time.deltaTime * Settings.RotationDamping);
            SpearData.CenterTransform.rotation = smoothedRotation;
        }

        public override void UpdateScale()
        {
            float scaleFactor = 0;

            if (SpearData.ShrinkRequest)
            {
                SpearData.loadTimer += Time.deltaTime;
            }

            if (!SpearData.WasExpandRequest) _startHoldingExpand = false;
            if (!_startHoldingExpand)
            {
                if (SpearData.ExpandRequest)
                {
                    StateSwitcher.SwitchState<FromUmbrellaToNormalTransition>();
                }
            }


            var currentVelocity = SpearData.Player.PlayerData.ControlledCollider.GetVelocity();
            var velocityBaseAdd = (Vector2) (SpearData.SpearConfig.UmbrellaYSpeed * SpearData.SpearScaler.HandlePoint.right) * Time.deltaTime;
            var velocityAdd = Vector3.Project(velocityBaseAdd, Vector2.up);
            if (velocityAdd.y < 0)  velocityAdd.y = 0;
            SpearData.Player.PlayerData.ControlledCollider.SetVelocity(currentVelocity + (Vector2) velocityAdd);


            SpearData.SpearScaler.ChangeScale(scaleFactor, Settings.MinShrink, Settings.MaxExpand);
        }
    }
}