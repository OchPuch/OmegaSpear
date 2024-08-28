using System;
using EnvironmentObjects.Triggers;
using GlobalManagers;
using UnityEngine;

namespace Saving
{
    public class CheckPoint : PlayerTrigger
    {
        [SerializeField] private bool clearCheckpointsInstead;
        private string _sceneName;
        private bool _set;
        public event Action CheckPointSet;
        
        protected override void Start()
        {
            base.Start();
            _sceneName = gameObject.scene.name;
        }

        protected override void OnPlayerEnter(Collider player1)
        {
            if (_set) return;
            if (clearCheckpointsInstead)
            {
                CheckpointManager.Instance.DeleteCheckpoint();
                return;
            }
            
            var saveData = Player.Player.Instance.GetCharacterSaveData();
            CheckpointManager.Instance.SetCheckpoint(new SaveData
            {
                sceneName = _sceneName,
                characterSaveData = saveData,
                seconds = TimeManager.Instance.LevelTimer
            }); 
            _set = true;
            CheckPointSet?.Invoke();
        }
    }
}

