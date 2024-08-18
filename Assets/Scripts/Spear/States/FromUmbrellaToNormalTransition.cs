using Effects;
using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear.States
{
    public class FromUmbrellaToNormalTransition : SpearState
    {
        private TransitionSettings Settings => SpearData.SpearConfig.FromUmbrellaToNormalTransition;
        private float _transitionTimer;
        private readonly float _transitionTime;
        private bool _specialActionCommited;
        private readonly ParticleFactory _particleFactory;
        
        public FromUmbrellaToNormalTransition(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
        {
            _particleFactory = new ParticleFactory(Settings.SpecialEffect);
            _transitionTime = Settings.Transition.keys[^1].time;
        }

        public override void Enter()
        {
            base.Enter();
            _transitionTimer = 0f;
            _specialActionCommited = false;
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
            SpearData.SpearScaler.SetScale(Settings.Transition.Evaluate(_transitionTimer), Settings.MinShrink, Settings.MaxExpand);
            if (_transitionTimer >= Settings.SpecialActionTime && !_specialActionCommited)
            {
                DoubleJump();
                _specialActionCommited = true;
            }
            if (_transitionTimer >= _transitionTime)
            {
                StateSwitcher.SwitchState<DefaultState>();
            }
        }

        private void DoubleJump()
        {
            float jumpScale = Mathf.Lerp(0, 1, SpearData.loadTimer / Config.TimeToMaxUmbrellaDoubleJump);
            var currentVelocity = SpearData.Player.PlayerData.ControlledCollider.GetVelocity();
            currentVelocity += (Vector2) SpearData.SpearScaler.HandlePoint.right * (Config.UmbrellaDoubleJump * jumpScale);
            SpearData.Player.PlayerData.ControlledCollider.SetVelocity(currentVelocity);
            _particleFactory.CreateParticleSystem(SpearData.SpearScaler.HandlePoint.position);

        }
    }
}