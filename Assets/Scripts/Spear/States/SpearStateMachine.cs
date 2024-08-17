using System.Collections.Generic;
using Spear.Data;
using StateMachine;

namespace Spear.States
{
    public class SpearStateMachine : StateMachine.StateMachine, IStateSwitcher
    {
        private SpearState SpearState => (SpearState) CurrentState;
        
        public SpearStateMachine(SpearData spearData, Spear spear)
        {
            States = new List<IState>(new List<SpearState>()
            {
                new DefaultState(this, spearData)
            });
        }
        
        public void SwitchState<TState>() where TState : IState
        {
            if (CurrentState is not global::Spear.States.SpearState) return;
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
            SpearState.Update();
        }
    }
}