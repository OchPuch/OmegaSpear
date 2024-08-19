using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear.States
{
    public class FromAnyToUmbrellaTransition : SpearState
    {
        private float _holdShrinkTime;
        private float _holdExpandTime;
        
        private float ShrinkFactor => Settings.ShrinkingSpeedByHolding.Evaluate(_holdShrinkTime) *
                                      Settings.ScalingMultiplier *
                                      Time.deltaTime;

        private float ExpandFactor => Settings.ExpandingSpeedByHolding.Evaluate(_holdExpandTime) *
                                      Settings.ScalingMultiplier * Time.deltaTime;
        
        private SpearStateSettings Settings => SpearData.SpearConfig.FromAnyToUmbrellaSettings;
        
        public FromAnyToUmbrellaTransition(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
        {
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
        
        
        public override void Update()
        {
        }

        public override void UpdateScale()
        {
            float scaleFactor = 0f;
            if (SpearData.Scale > Settings.MaxExpand)
            {
                _holdShrinkTime += Time.deltaTime;
                scaleFactor = -ShrinkFactor;
                if (SpearData.Scale + scaleFactor < Settings.MinShrink)
                {
                    StateSwitcher.SwitchState<UmbrellaState>();
                    return;
                }

            }
            else if (SpearData.Scale < Settings.MinShrink)
            {
                _holdExpandTime += Time.deltaTime;
                scaleFactor = ExpandFactor;
                if (SpearData.Scale + scaleFactor > Settings.MaxExpand)
                {
                    StateSwitcher.SwitchState<UmbrellaState>();
                    return;
                }
            }
            else
            {
                StateSwitcher.SwitchState<UmbrellaState>();
                return;
            }
            
            SpearData.SpearScaler.ChangeScale(scaleFactor, Settings.MinShrink, Settings.MaxExpand);
        }
    }
}