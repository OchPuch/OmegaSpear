﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GlobalManagers
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance { get; private set; }
        public bool IsPaused { get; private set; }
        
        public event Action Paused;
        public event Action Resumed;

        public void Init()
        {
            if (Instance == this) return;
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            transform.SetParent(null);
            DontDestroyOnLoad(this);
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Paused = null;
                Resumed = null;
                Instance = null;
            }
        }

        [Button("Pause")]
        public void Pause()
        {
            IsPaused = true;
            Paused?.Invoke();
        }

        [Button("Resume")]
        public void Resume()
        {
            IsPaused = false;
            Resumed?.Invoke();
        }
    }
}