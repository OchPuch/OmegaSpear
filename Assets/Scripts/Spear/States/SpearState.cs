using Spear.Data;
using StateMachine;

namespace Spear.States
{
    public abstract class SpearState : IState
    {
        protected SpearData SpearData;
        protected IStateSwitcher StateSwitcher;
        
        public SpearState(IStateSwitcher stateSwitcher, SpearData spearData)
        {
            SpearData = spearData;
            StateSwitcher = stateSwitcher;
        }
        
        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void Update()
        {
            
        }

        protected virtual void HandleRotation()
        {
            
        }
    }
}