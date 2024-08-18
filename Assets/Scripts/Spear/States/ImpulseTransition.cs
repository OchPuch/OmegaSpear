using Effects;
using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear.States
{
    public class ImpulseTransition : SpearState
    {
        private TransitionSettings Settings => SpearData.SpearConfig.ImpulseTransition;
        private float _transitionTimer;
        private readonly float _transitionTime;
        private bool _specialActionCommited;
        private readonly ParticleFactory _particleFactory;
        public ImpulseTransition(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
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
                CreateImpulse();
            }
            
            if (_transitionTimer >= _transitionTime)
            {
                StateSwitcher.SwitchState<DefaultState>();
            }
        }

        private void CreateImpulse()
        {
            _specialActionCommited = true;
            Vector2 spawnEffectPosition = SpearData.SpearScaler.TipPoint.position;
            if (Physics.Raycast(SpearData.CenterTransform.position, SpearData.CenterTransform.right, out var hit, Settings.MaxExpand, SpearData.SpearConfig.HitMask))
            {
                float force = SpearData.SpearConfig.ImpulsePlayerSpeed;
                spawnEffectPosition = hit.point;
                
                if (hit.collider.TryGetComponent<Rigidbody>(out var rb))
                {
                    if (!rb.isKinematic)
                    {
                        force /= 2;
                        rb.AddForce(SpearData.CenterTransform.right * force, ForceMode.Impulse);
                    }
                }
                
                var currentVelocity = SpearData.Player.PlayerData.ControlledCollider.GetVelocity();
                if (currentVelocity.y < 0) currentVelocity.y = 0f;
                currentVelocity +=  (Vector2) (-SpearData.CenterTransform.right) * force;
                SpearData.Player.PlayerData.ControlledCollider.SetVelocity(currentVelocity);
            }
            
            _particleFactory.CreateParticleSystem(spawnEffectPosition);
        }
    }
}