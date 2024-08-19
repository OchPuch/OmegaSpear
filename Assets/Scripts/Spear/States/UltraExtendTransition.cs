﻿using Effects;
using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear.States
{
    public class UltraExtendTransition : SpearState
    {
        private TransitionSettings Settings => SpearData.SpearConfig.UltraExtendTransition;
        private float _transitionTimer;
        private float _maxScale;
        private readonly float _transitionTime;
        private readonly ParticleFactory _particleFactory;
        private bool _shouldBeLocked;
        
        public UltraExtendTransition(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
        {
            _transitionTime = Settings.Transition.keys[^1].time;
            _particleFactory = new ParticleFactory(Settings.SpecialEffect);
            if (SpearData.TipPoint.IsLocked) SpearData.TipPoint.UnLock();
        }
        
        public override void Enter()
        {
            base.Enter();
            _transitionTimer = 0f;
            CalculateMaxScale();
        }

        public override void Update()
        {
        }


        private void CalculateMaxScale()
        {
            _maxScale = Settings.MaxExpand;
            if (Physics.Raycast(SpearData.CenterTransform.position, SpearData.CenterTransform.right, out var hit, Settings.MaxExpand, SpearData.SpearConfig.HitMask))
            {
                var hitPoint = hit.point;
                _particleFactory.CreateParticleSystem(hitPoint);
                _maxScale = Vector2.Distance(SpearData.SpearScaler.HandlePoint.position, hitPoint);
                _shouldBeLocked = true;
            }
            
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
            _transitionTimer += Time.deltaTime;
            if (_shouldBeLocked)
            {
                SpearData.SpearScaler.SetScale(_maxScale, Settings.MinShrink, _maxScale);
                if (SpearData.TipPoint.Lock())
                {
                    StateSwitcher.SwitchState<DefaultState>();
                }
            }
            else
            {
                SpearData.SpearScaler.SetScale(Settings.Transition.Evaluate(_transitionTimer), Settings.MinShrink, _maxScale);
            }
            
            
            if (_transitionTimer >= _transitionTime)
            {
                StateSwitcher.SwitchState<DefaultState>();
            }
        }

        
    }
}