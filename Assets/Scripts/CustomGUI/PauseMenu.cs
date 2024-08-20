using System;
using GlobalManagers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace CustomGUI
{
    public class PauseMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public GameObject pauseMenu;
        [SerializeField] private SceneField mainMenu;
        [SerializeField] private Button restartButton;
        [SerializeField] private bool allowRestart = true;

        public Slider sfx;
        public Slider music;

        private void Start()
        {
            sfx.value = PlayerPrefs.GetFloat("SFX", 0.9f);
            music.value = PlayerPrefs.GetFloat("Music", 0.9f);
        
            UpdateMusicVolume(music);
            UpdateSfxVolume(sfx);

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
        
        public void UpdateSfxVolume(Slider slider)
        {
            float volume = Mathf.Lerp(-80f, -20f, slider.value);
            audioMixer.SetFloat("SFX", volume);
            PlayerPrefs.SetFloat("SFX", slider.value);
        }

        public void UpdateMusicVolume(Slider slider)
        {
            float volume = Mathf.Lerp(-80f, -20f, slider.value);
            audioMixer.SetFloat("Music", volume);
            PlayerPrefs.SetFloat("Music", slider.value);
        }

        public void DeleteSaves()
        {
            PlayerPrefs.DeleteAll();
        }


    }
}
