using Spear.Data;
using StateMachine;

namespace Spear.States.General
{
    public abstract class SpearState : IState
    {
        protected SpearConfig Config => SpearData.SpearConfig;
        protected SpearData SpearData;
        protected IStateSwitcher StateSwitcher;
        protected Spear Spear;

        protected SpearState(IStateSwitcher stateSwitcher, SpearData spearData, Spear spear)
        {
            SpearData = spearData;
            StateSwitcher = stateSwitcher;
            Spear = spear;
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

        public virtual void HandleRotation()
        {
            
        }

        public abstract void UpdateScale();
    }
}