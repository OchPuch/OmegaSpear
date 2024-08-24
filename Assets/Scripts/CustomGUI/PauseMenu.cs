using GlobalManagers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace CustomGUI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private SceneField mainMenu;
        [SerializeField] private Button restartButton;
        [SerializeField] private bool allowRestart = true;

        public Slider sfx;
        public Slider music;

        private void Start()
        {
            sfx.value = SoundManager.Instance.GetSoundEffectsVolume01();
            music.value = SoundManager.Instance.GetMusicVolume01();

            restartButton.interactable = allowRestart;
            pauseMenu.SetActive(PauseManager.Instance.IsPaused);
            PauseManager.Instance.Resume();
        }


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (PauseManager.Instance.IsPaused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            PauseManager.Instance.Pause();
            pauseMenu.SetActive(true);
        }

        public void UnPause()
        {
            PauseManager.Instance.Resume();
            pauseMenu.SetActive(false);
        }
    
        public void LoadMainMenu()
        {
            pauseMenu.SetActive(false);
            SceneManager.LoadScene(mainMenu);
            PauseManager.Instance.Resume();

        }

        public void Restart()
        {
            if (CheckpointManager.Instance.TryGetLastCheckPoint(out var data))
            {
                LevelManager.Instance.LoadCheckpoint(data);
            }
            PauseManager.Instance.Resume();
        }

        public void DeleteSaves()
        {
            PlayerPrefs.DeleteAll();
        }

        public void UpdateSoundEffectsVolume(Slider slider)
        {
            SoundManager.Instance.UpdateSfxVolume(slider.value);
        }
        
        public void UpdateMusicVolume(Slider slider)
        {
            SoundManager.Instance.UpdateMusicVolume(slider.value);
        }


    }
}
