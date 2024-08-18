﻿using System;
using System.Collections;
using UnityEngine;

namespace GlobalManagers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }
        
        private float _pausedTimeScale = 1.0f;
        private Coroutine _freezingEffect;

        public void Init()
        {
            if (Instance == this) return;
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            if (PauseManager.Instance is null) return;
            PauseManager.Instance.Paused += OnPause;
            PauseManager.Instance.Resumed += OnResume;
        }

        public void FreezeTimeEffectStart(float effectTime)
        {
            if (_freezingEffect is not null) StopCoroutine(_freezingEffect);
            _freezingEffect = StartCoroutine(FreezeTimeForSeconds(effectTime));
        }

        public void StopFreezeTimeEffect()
        {
            if (_freezingEffect is not null)
            {
                if (PauseManager.Instance.IsPaused) _pausedTimeScale = 1.0f;
                else Time.timeScale = 1.0f;
            }
        }

        private IEnumerator FreezeTimeForSeconds(float time)
        {
            float elapsedTime = 0;
            Time.timeScale = 0;
            while (elapsedTime < time)
            {
                if (PauseManager.Instance.IsPaused)
                {
                    yield return null;
                    continue;
                }
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = 1.0f;
            _freezingEffect = null;
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            
            Instance = null;
            if (PauseManager.Instance is null) return;
            PauseManager.Instance.Paused -= OnPause;
            PauseManager.Instance.Resumed -= OnResume;
        }
        
        private void OnResume()
        {
            Time.timeScale = _pausedTimeScale;
        }

        private void OnPause()
        {
            _pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
    }
}