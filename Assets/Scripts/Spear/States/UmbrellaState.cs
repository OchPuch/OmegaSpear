﻿using Spear.Data;
using Spear.States.General;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Spear.States
{
    public class UmbrellaState : SpearState
    {
        
        private SpearStateSettings Settings => SpearData.SpearConfig.UmbrellaSettings;

        public UmbrellaState(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher,
            spearData, spear)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (SpearData.TipPoint.IsLocked) SpearData.TipPoint.UnLock();
            SpearData.AudioSource.PlayOneShot(Settings.SpecialSound1);
        }

        public override void Update()
        {
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
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                StateSwitcher.SwitchState<FromUmbrellaToNormalTransition>();
                return;
            }
            
            var umbrellaCharge01 = Mathf.Clamp01(SpearData.UmbrellaCharge / Config.UmbrellaMaxTimeCharge);
            var chargeScaleSpeed = Config.UmbrellaYSpeedByCharge.Evaluate(umbrellaCharge01);
            var currentVelocity = SpearData.Player.PlayerData.ControlledCollider.GetVelocity();
            var velocityBaseAdd = (Vector2) (SpearData.SpearScaler.HandlePoint.right * (SpearData.SpearConfig.UmbrellaYSpeed * chargeScaleSpeed * Time.deltaTime));
            if (velocityBaseAdd.y < 0)  velocityBaseAdd.y = 0;
            SpearData.Player.PlayerData.ControlledCollider.SetVelocity(currentVelocity + velocityBaseAdd);
            SpearData.SpearScaler.SetScale(Settings.MaxExpand, Settings.MinShrink, Settings.MaxExpand);
            SpearData.SpearScaler.UpdateFat(Settings.FatingSpeedWhileExpanding.Evaluate(umbrellaCharge01));
            if (SpearData.Player.PlayerData.ControlledCollider.IsGrounded())
            {
                SpearData.AddUmbrellaCharge(Time.deltaTime);
            }
            else
            {
                SpearData.AddUmbrellaCharge(-Time.deltaTime);
            }
            
        }
    }
}