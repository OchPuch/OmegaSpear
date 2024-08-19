using System.Collections.Generic;
using Spear.Data;
using StateMachine;

namespace Spear.States.General
{
    public class SpearStateMachine : StateMachine.StateMachine, IStateSwitcher
    {
        private SpearState SpearState => (SpearState) CurrentState;
        
        public SpearStateMachine(SpearData spearData, Spear spear)
        {
            States = new List<IState>(new List<SpearState>()
            {
                new DefaultState(this, spearData, spear),
                new OnceLoadedState(this, spearData, spear),
                new TwiceLoadedState(this, spearData, spear),
                new ImpulseTransition(this, spearData, spear),
                new UltraExtendTransition(this, spearData, spear),
                new UmbrellaState(this, spearData, spear),
                new FromUmbrellaToNormalTransition(this, spearData, spear),
                new FromAnyToUmbrellaTransition(this, spearData, spear)
                
            });

            CurrentState = States[0];
            CurrentState.Enter();
        }
        
        public void SwitchState<TState>() where TState : IState
        {
            if (CurrentState is not global::Spear.States.General.SpearState) return;
            if (CurrentState is TState) return;
            var newState = (SpearState) States.Find(state => state is TState);
            CurrentState.Exit();
            OnStateExited(CurrentState);
            CurrentState = newState;
            CurrentState.Enter();
            OnStateEntered(CurrentState);
        }

        public void Update()
        {
            SpearState.HandleRotation();
            SpearState.Update();
            SpearState.UpdateScale();
        }
    }
}