using GlobalManagers;
using Saving;
using UnityEngine;

namespace EnvironmentObjects.Triggers
{
    public class CheckPoint : PlayerTrigger
    {
        [SerializeField] private bool clearCheckpointsInstead;
        private string _sceneName;
        
        protected override void Start()
        {
            base.Start();
            _sceneName = gameObject.scene.name;
        }

        protected override void OnPlayerEnter(Collider player1)
        {
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
        }
    }
}

