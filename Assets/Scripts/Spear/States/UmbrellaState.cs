using Spear.Data;
using Spear.States.General;
using StateMachine;
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
        public UmbrellaState(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
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

            if (VectorUtils.AreCodirected(Vector2.up, SpearData.SpearScaler.HandlePoint.right))
            {
                var currentVelocity = SpearData.Player.PlayerData.ControlledCollider.GetVelocity();
                if (currentVelocity.y < 0)
                {
                    var velocityY = Vector3.Project(SpearData.SpearScaler.HandlePoint.right * Config.UmbrellaYSpeed, Vector2.up);
                    currentVelocity.y = velocityY.y;
                    SpearData.Player.PlayerData.ControlledCollider.SetVelocity(currentVelocity);
                }
            }
            
            SpearData.SpearScaler.ChangeScale(scaleFactor, Settings.MinShrink, Settings.MaxExpand);
        }
    }
}