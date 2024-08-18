using Effects;
using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear.States
{
    public class DefaultState : SpearState
    {
        private SpearStateSettings Settings => SpearData.SpearConfig.NormalSettings;
        private float _holdShrinkTime;
        private float _holdExpandTime;
        
        private bool _consistentShrink;
        private bool _stopped;

        private float ShrinkFactor => Settings.ShrinkingSpeedByHolding.Evaluate(_holdShrinkTime) *
                                      Settings.ScalingMultiplier *
                                      Time.deltaTime;

        private float ExpandFactor => Settings.ExpandingSpeedByHolding.Evaluate(_holdExpandTime) *
                                      Settings.ScalingMultiplier * Time.deltaTime;

        private readonly ParticleFactory _particleFactory;

        public DefaultState(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher,
            spearData, spear)
        {
            _particleFactory = new ParticleFactory(Settings.StopEffect);
        }

        public override void Enter()
        {
            base.Enter();
            _holdExpandTime = 0;
            _holdShrinkTime = 0;

            _consistentShrink = false;
            _stopped = true;
        }


        public override void HandleRotation()
        {
            base.HandleRotation();
            if (SpearData.TipPoint.IsLocked) return;
            Quaternion desiredRotQ = Quaternion.Euler(0, 0, SpearData.desiredRotation);
            Quaternion smoothedRotation = Quaternion.Lerp(SpearData.CenterTransform.rotation, desiredRotQ,
                Time.deltaTime * Settings.RotationDamping);
            SpearData.CenterTransform.rotation = smoothedRotation;
        }

        public override void UpdateScale()
        {
            float scaleFactor = 0;
            
            if (SpearData.TipPoint.IsLocked)
            {
                var projectedVelocity = Vector3.Project(SpearData.Player.PlayerData.ControlledCollider.GetVelocity(),
                    SpearData.SpearScaler.HandlePoint.up);
                SpearData.Player.PlayerData.ControlledCollider.SetVelocity(projectedVelocity);
                
                if (!SpearData.ShrinkRequest)
                {
                    _holdShrinkTime = 0;
                }
                
                if (SpearData.ShrinkRequest && !SpearData.ExpandRequest)
                {
                    _holdShrinkTime += Time.deltaTime;
                    if (SpearData.TipPoint.IsInHardGround)
                    {
                        var velocityToPoint = SpearData.SpearScaler.HandlePoint.right * (Config.StuckShrinkSpeedMultiplier * ShrinkFactor) / Time.deltaTime;
                        var currentVelocity = SpearData.Player.PlayerData.ControlledCollider.GetVelocity();
                        SpearData.Player.PlayerData.ControlledCollider.SetVelocity(currentVelocity + (Vector2) velocityToPoint);
                        if (SpearData.Scale <= Config.UnstuckFromGroundScale)
                        {
                            SpearData.TipPoint.UnLock();
                        }
                    }
                    else if (_holdShrinkTime > Config.ShrinkHoldTimeToUnlock)
                    {
                        SpearData.TipPoint.UnLock();
                    }
                }
                
                return;
            }
            
            if (SpearData.ExpandRequest && SpearData.ShrinkRequest)
            {
                if (SpearData.TipPoint.Lock())
                    return;
            }
            
            
            if (SpearData.Scale < Settings.MinShrink)
            {
                scaleFactor += ExpandFactor;
            }

            if (SpearData.Scale > Settings.MaxExpand)
            {
                scaleFactor -= ShrinkFactor;
            }
            
            if (SpearData.ExpandRequest)
            {
                if (!SpearData.ShrinkRequest) _stopped = false;
                scaleFactor += ExpandFactor;
                _holdExpandTime += Time.deltaTime;
            }
            else
            {
                _holdExpandTime = 0;
            }


            if (SpearData.ShrinkRequest)
            {
                _consistentShrink = !SpearData.ExpandRequest;
                scaleFactor -= ShrinkFactor;
                _holdShrinkTime += Time.deltaTime;
            }
            else
            {
                _holdShrinkTime = 0f;
            }


            if (_consistentShrink && !_stopped && SpearData.Scale + scaleFactor < Settings.MinScaleToChangeState)
            {
                _stopped = true;
                //TODO: PlayEffect 
                //Todo: Instantiate Particle
                _particleFactory.CreateParticleSystem(SpearData.SpearScaler.HandlePoint.position);
            }

            if (SpearData.Scale + scaleFactor < Settings.MinScaleToChangeState && SpearData.ShrinkRequest)
            {
                StateSwitcher.SwitchState<OnceLoadedState>();
            }
            else
            {
                SpearData.SpearScaler.ChangeScale(scaleFactor, Settings.MinShrink, Settings.MaxExpand);
            }
        }
    }
}