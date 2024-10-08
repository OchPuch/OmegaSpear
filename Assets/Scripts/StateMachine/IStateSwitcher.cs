﻿namespace StateMachine
{
    public interface IStateSwitcher
    { 
        void SwitchState<TState>() where TState : IState;
    }
}