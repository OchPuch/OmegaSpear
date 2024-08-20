using CommonObjects;
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
        public SpearData SpearData { get; private set; }

        private Camera _mainCamera;

        public void Init(SpearData spearData)
        {
            SpearData = spearData;
            _spearStateMachine = new SpearStateMachine(spearData, this);
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            SpearData.UpdateInput();
            CalculateDesiredRotation();
            _spearStateMachine.Update();
        }

        private void LateUpdate()
        {
            SpearData.SaveLastInput();
        }

        private void CalculateDesiredRotation()
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition) - (Vector2)transform.position;
            SpearData.desiredRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        private void PrintCurrentState(IState state)
        {
            Debug.Log(_spearStateMachine.CurrentStateName());
        }
    }
}