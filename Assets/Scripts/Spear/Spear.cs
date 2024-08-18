using CommonObjects;
using Sirenix.OdinInspector;
using Spear.Data;
using Spear.States.General;
using StateMachine;
using UnityEngine;

namespace Spear
{
    public class Spear : GamePlayBehaviour
    {
        public StateMachine.StateMachine StateMachine => _spearStateMachine;
        private SpearStateMachine _spearStateMachine;
        private SpearData _spearData;

        private Camera _mainCamera;

        public void Init(SpearData spearData)
        {
            _spearData = spearData;
            _spearStateMachine = new SpearStateMachine(spearData, this);
            _mainCamera = Camera.main;
            _spearStateMachine.StateEntered += PrintCurrentState;
        }

        private void Update()
        {
            _spearData.UpdateInput();
            CalculateDesiredRotation();
            _spearStateMachine.Update();
        }

        private void LateUpdate()
        {
            _spearData.SaveLastInput();
        }

        private void CalculateDesiredRotation()
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition) - (Vector2)transform.position;
            _spearData.desiredRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        private void PrintCurrentState(IState state)
        {
            Debug.Log(_spearStateMachine.CurrentStateName());
        }
    }
}