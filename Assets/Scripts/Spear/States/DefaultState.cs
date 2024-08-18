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

        private bool _startHoldingExpand;

        private bool _consistentShrink;
        private bool _stopped;

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

            _startHoldingExpand = SpearData.WasExpandRequest;
            _consistentShrink = false;
            _stopped = true;
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

            if (!SpearData.WasExpandRequest) _startHoldingExpand = false;
            if (!_startHoldingExpand)
            {
                if (SpearData.ExpandRequest)
                {
                    if (!SpearData.ShrinkRequest) _stopped = false;
                    scaleFactor += Settings.ExpandingSpeedByHolding.Evaluate(_holdExpandTime) *
                                   Settings.ScalingMultiplier * Time.deltaTime;
                    _holdExpandTime += Time.deltaTime;
                }
                else
                {
                    _holdExpandTime = 0;
                }
            }


            if (SpearData.ShrinkRequest)
            {
                _consistentShrink = !SpearData.ExpandRequest;
                scaleFactor -= Settings.ShrinkingSpeedByHolding.Evaluate(_holdShrinkTime) * Settings.ScalingMultiplier *
                               Time.deltaTime;
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