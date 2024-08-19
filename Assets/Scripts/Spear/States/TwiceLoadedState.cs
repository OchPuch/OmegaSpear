using Effects;
using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear.States
{
    public class TwiceLoadedState : SpearState
    {
        private float _expandTimer = 0f;
        private readonly ParticleFactory _particleFactory;
        private float _shakeTimer;
        private bool _passedShakeIntro;
        private readonly float _loopShakeTime;
        private readonly float _introShakeTime;
        
        private SpearStateSettings Settings => SpearData.SpearConfig.TwiceLoadedSettings;
        public TwiceLoadedState(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
        {
            _particleFactory = new ParticleFactory(Settings.StopEffect);
            _introShakeTime = Config.ShakeIntro.keys[^1].time;
            _loopShakeTime = Config.ShakeLoop.keys[^1].time;
        }

        public override void Enter()
        {
            base.Enter();
            _expandTimer = 0f;
            _passedShakeIntro = false;
            _shakeTimer = 0f;
            if (SpearData.TipPoint.IsLocked) SpearData.TipPoint.UnLock();
        }

        public override void HandleRotation()
        {
            base.HandleRotation();
            Quaternion desiredRotQ = Quaternion.Euler(0,0, SpearData.desiredRotation);
            Quaternion smoothedRotation = Quaternion.Lerp(SpearData.CenterTransform.rotation, desiredRotQ, Time.deltaTime * (Settings.RotationDamping - SpearData.Scale));
            SpearData.CenterTransform.rotation = smoothedRotation;
        }

        public override void UpdateScale()
        {
            float scaleFactor = 0;
            if (SpearData.ExpandRequest)
            {
                if (!SpearData.WasExpandRequest && SpearData.Scale < Settings.MinScaleToChangeState)
                {
                    StateSwitcher.SwitchState<UltraExtendTransition>();
                    return;
                }
            }
            
            if (SpearData.ShrinkRequest)
            {
                scaleFactor -= Settings.ShrinkingSpeedByHolding.Evaluate(SpearData.loadTimer) * Settings.ScalingMultiplier * Time.deltaTime;
                SpearData.loadTimer += Time.deltaTime;
                _shakeTimer += Time.deltaTime;
                _expandTimer = 0f;
            }
            else
            {
                scaleFactor += Settings.ExpandingSpeedByHolding.Evaluate(_expandTimer) * Settings.ScalingMultiplier * Time.deltaTime;
                _expandTimer += Time.deltaTime;
                SpearData.loadTimer -= Time.deltaTime;
                _shakeTimer = 0f;
                _passedShakeIntro = false;
            }
            
            if (scaleFactor > 0)
            {
                if (SpearData.Scale + scaleFactor > Settings.MaxExpand)
                {
                    SpearData.AudioSource.PlayOneShot(Settings.SpecialSound1);
                    _particleFactory.CreateParticleSystem(SpearData.SpearScaler.HandlePoint.position);
                    SpearData.SpearScaler.ChangeScale(scaleFactor, Settings.MinShrink, Settings.MaxExpand);
                    StateSwitcher.SwitchState<DefaultState>();
                }
            }
            
            if (_shakeTimer > _introShakeTime && !_passedShakeIntro)
            {
                _shakeTimer = 0;
                _passedShakeIntro = true;
            }
            
            if (_shakeTimer > _loopShakeTime && _passedShakeIntro)
            {
                _shakeTimer = 0f;
            }

            SpearData.SpearScaler.UpdateFat(_passedShakeIntro
                ? Config.ShakeLoop.Evaluate(_shakeTimer)
                : Config.ShakeIntro.Evaluate(_shakeTimer));
            
            SpearData.SpearScaler.ChangeScale(scaleFactor, Settings.MinShrink, Settings.MaxExpand);
        }
    }
}