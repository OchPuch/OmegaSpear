using UnityEngine;
using UnityEngine.Audio;

namespace GlobalManagers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private const string MusicKey = "Music";
        private const string SoundEffectsKey = "SFX";
        
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float volumeChangeFactor;
        
        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
                UpdateSfxVolume(GetSoundEffectsVolume01());
                UpdateMusicVolume(GetMusicVolume01());
            }
            else if (Instance != null)
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
        
        
        public void UpdateSfxVolume(float value)
        {
            float volume = Mathf.Lerp(-80f, 20f, Mathf.Pow(value, volumeChangeFactor));
            audioMixer.SetFloat(SoundEffectsKey, volume);
            PlayerPrefs.SetFloat(SoundEffectsKey, value);
        }

        public void UpdateMusicVolume(float value)
        {
            float volume = Mathf.Lerp(-80f, 20f, Mathf.Pow(value, volumeChangeFactor));
            audioMixer.SetFloat(MusicKey, volume);
            PlayerPrefs.SetFloat(MusicKey, value);
        }

        public float GetSoundEffectsVolume01()
        {
            return PlayerPrefs.GetFloat(SoundEffectsKey, 0.5f);
        }
        
        public float GetMusicVolume01()
        {
            return PlayerPrefs.GetFloat(MusicKey, 0.5f);
        }
        
    }
}