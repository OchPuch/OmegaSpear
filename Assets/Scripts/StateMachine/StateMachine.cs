using System;
using System.Collections.Generic;

namespace StateMachine
{
    public abstract class StateMachine
    {
        protected List<IState> States;
        protected IState CurrentState;
        
        public event Action<IState> StateEntered;
        public event Action<IState> StateExited;
        
        protected void OnStateEntered(IState state)
        {
            StateEntered?.Invoke(state);
        }
        
        protected void OnStateExited(IState state)
        {
            StateExited?.Invoke(state);
        }
        
        public bool IsInState<TState>() where TState : IState
        {
            return CurrentState is TState;
        }
        
        public string CurrentStateName()
        {
            return (CurrentState.GetType().Name);
        }
        
        
    }
}