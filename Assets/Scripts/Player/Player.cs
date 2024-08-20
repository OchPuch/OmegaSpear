using System;
using CommonObjects;
using GlobalManagers;
using Player.Data;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Player : GamePlayBehaviour
    {
        public static Player Instance { get; private set; }
        public IPlayerEvents PlayerEvents => _data.PlayerEvents;
        private PlayerData _data;
        public PlayerData PlayerData => _data;

        private bool _loadSaveRequest;

        public void Init(PlayerData playerData)
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _data = playerData;
            PlayerEvents.Died += OnDeath;
        }

        protected override void Start()
        {
            base.Start();
            if (CheckpointManager.Instance.TryGetLastCheckPoint(out var data))
            {
                _loadSaveRequest = true;
            }
        }

        private void FixedUpdate()
        {
            if (_loadSaveRequest)
            {
                ApplyCheckpoint(CheckpointManager.Instance.GetLastCheckpoint());
                _loadSaveRequest = false;
            }
        }

        private void ApplyCheckpoint(SaveData saveData)
        {
            //Check if scene is loaded
            if (saveData.sceneName is null or "") return;
            if (!SceneManager.GetSceneByName(saveData.sceneName).isLoaded) return;
            var characterSave = saveData.characterSaveData;
            PlayerData.ControlledCollider.SetPosition(new Vector3(characterSave.positionX, characterSave.positionY));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void OnDeath()
        {
            gameObject.SetActive(false);
        }

        public SaveData.CharacterSaveData GetCharacterSaveData()
        {
            return new SaveData.CharacterSaveData()
            {
                positionX = transform.position.x,
                positionY = transform.position.y
            };
        }
    }
}