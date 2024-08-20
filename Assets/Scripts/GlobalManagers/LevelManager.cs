using System;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace GlobalManagers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        [SerializeField] private SceneField persistentGameplayScene;
        
        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        public void LoadNewLevel(SceneField level)
        {
            SceneManager.LoadScene(persistentGameplayScene);
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
        }
        
        public void LoadNewLevel(string level)
        {
            if (level is null or "") 
            {
                Debug.LogError("Level name is null or empty");
                return;
            }
            SceneManager.LoadScene(persistentGameplayScene);
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
        }
        
        
        public void LoadCheckpoint(SaveData saveData)
        {
            LoadNewLevel(saveData.sceneName);
        }

        public void LoadScenes(SceneField[] scenesToLoad) {
            foreach (var scene in scenesToLoad) {
                if (!SceneUtils.IsSceneLoaded(scene)) {
                    SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                }
            }
        }

        public void UnloadScenes(SceneField[] scenesToUnload) {
            foreach (var t in scenesToUnload) {
                if (SceneUtils.IsSceneLoaded(t)) {
                    SceneManager.UnloadSceneAsync(t);
                }
            }
        }
        
        public void UnloadScenes(Scene[] scenesToUnload) {
            foreach (var t in scenesToUnload) {
                if (t.isLoaded) {
                    SceneManager.UnloadSceneAsync(t);
                }
            }
        }
        
        
    }
}