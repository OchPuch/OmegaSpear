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
        private readonly float _transitionTime;
        
        public UltraExtendTransition(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear) : base(stateSwitcher, spearData, spear)
        {
            _transitionTime = Settings.Transition.keys[^1].time;
        }
        
        public override void Enter()
        {
            base.Enter();
            _transitionTimer = 0f;
        }

        public override void UpdateScale()
        {
            _transitionTimer += Time.deltaTime;
            SpearData.SpearScaler.SetScale(Settings.Transition.Evaluate(_transitionTimer));
            if (_transitionTimer >= _transitionTime)
            {
                //TODO: IDK bruh
                StateSwitcher.SwitchState<DefaultState>();
            }
        }
    }
}