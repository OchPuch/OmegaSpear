using GlobalManagers;
using Music;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace CustomGUI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MusicSwitchData musicSwitchData;
        [SerializeField] private Button continueButton;
        [Header("Scenes")]
        [SerializeField] private SceneField persistentScene;
        [SerializeField] private SceneField firstLevelScene;
        [Space(10)]
        [SerializeField] private SceneField persistentPoor;
        [SerializeField] private SceneField tutorialScene;

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
            {
                PlayerPrefs.SetString("PlayerName", "Player");
                LoadTutorial();
            }
            else
            {
                if (!CheckpointManager.Instance.LoadCheckpoint())
                {
                    continueButton.interactable = false;
                }
            }
        }

        public void StartNewGame()
        {
            MusicManager.Instance.SwitchMusic(musicSwitchData);
            SceneManager.LoadScene(persistentScene);
            SceneManager.LoadScene(firstLevelScene, LoadSceneMode.Additive);
        }

        public void Continue()
        { 
            MusicManager.Instance.SwitchMusic(musicSwitchData);
            SceneManager.LoadScene(persistentScene);
            SceneManager.LoadScene(CheckpointManager.Instance.GetLastCheckpoint().sceneName, LoadSceneMode.Additive);
        }
        
        private void LoadTutorial()
        {
            SceneManager.LoadScene(persistentPoor);
            SceneManager.LoadScene(tutorialScene, LoadSceneMode.Additive);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}