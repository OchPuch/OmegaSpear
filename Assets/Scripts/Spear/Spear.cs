using System;
using CommonObjects;
using Spear.Data;
using Spear.States;
using UnityEngine;

namespace Spear
{
    public class Spear : GamePlayBehaviour
    {
        public StateMachine.StateMachine StateMachine => _spearStateMachine;
        private SpearStateMachine _spearStateMachine;

        [SerializeField] private SpearData spearData;

        public void Init()
        {
            _spearStateMachine = new SpearStateMachine(spearData, this);
        }

        #region StateMachineCalls

        private void Update()
        {
            _spearStateMachine.Update();
        }

        #endregion

    }
}
