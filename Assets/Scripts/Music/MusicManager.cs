using System;
using System.Collections.Generic;
using System.Linq;
using Music;
using UnityEngine;

namespace GlobalManagers
{
    /// <summary>
    /// This class is responsible for playing music tracks and managing their transitions.
    /// And only for that. For effects and snapshots use AudioMixerManager or smth like that.
    /// TODO: Create AudioMixerManager
    /// </summary>
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [SerializeField] private MusicPlayer musicPlayerPrefab;
        private readonly List<MusicPlayer> _musicPlayers = new List<MusicPlayer>();

        private Coroutine _crossFadeMusicTracksCoroutine;
        
        private MusicSwitchData _lastMusicSwitchData;

        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
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

        public void SwitchMusic(MusicSwitchData musicSwitchData)
        {
            if (musicSwitchData == null)
            {
                Debug.LogError("MusicSwitchData is null");
                return;
            }

            if (musicSwitchData.newMusicTrack == null)
            {
                Debug.LogError("MusicTrack is null");
                return;
            }

            if (_lastMusicSwitchData == musicSwitchData)
            {
                Debug.LogWarning("This music switch data is already playing");
                return;
            }
            
            _lastMusicSwitchData = musicSwitchData;
            
            if (_musicPlayers.Count == 0)
            {
                CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState);
                return;
            }
            
            if (_musicPlayers.Count == 1)
            {
                MusicPlayer currentMusicPlayer = _musicPlayers.First();

                if (musicSwitchData.synchronizeWithCurrentMusic)
                {
                    switch (musicSwitchData.transitionType)
                    {
                        case MusicTransitionType.Cut:
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, currentMusicPlayer.GetCurrentState(), 0,
                                currentMusicPlayer.GetCurrentPartPlayingTime());
                            currentMusicPlayer.ThisPlayerIsDone();
                            break;
                        case MusicTransitionType.CrossFade:
                            if (currentMusicPlayer.GetMusicTrack().GetClip(currentMusicPlayer.GetCurrentState()) ==
                                musicSwitchData.newMusicTrack.GetClip(currentMusicPlayer.GetCurrentState()) ||
                                musicSwitchData.crossFadeTime <= 0)
                            {
                                CreateNewMusicPlayer(musicSwitchData.newMusicTrack,
                                    currentMusicPlayer.GetCurrentState(), 0,
                                    currentMusicPlayer.GetCurrentPartPlayingTime());
                                currentMusicPlayer.ThisPlayerIsDone();
                                break;
                            }

                            currentMusicPlayer.FullFadeOut(musicSwitchData.crossFadeTime);
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, currentMusicPlayer.GetCurrentState(), 0,
                                currentMusicPlayer.GetCurrentPartPlayingTime(), musicSwitchData.crossFadeTime);
                            break;
                        default:
                            Debug.LogError(
                                "SynchronizeWithCurrentMusic is true, but transition type is not Cut or CrossFade");
                            break;
                    }
                }
                else
                {
                    if (_musicPlayers.Last().GetMusicTrack() == musicSwitchData.newMusicTrack)
                    {
                        _musicPlayers.Last().ChangeMusicState(musicSwitchData.startFromState, musicSwitchData.transitionType, musicSwitchData.crossFadeTime);
                        return;
                    }
                    
                    switch (musicSwitchData.transitionType)
                    {
                        case MusicTransitionType.Cut:
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState);
                            currentMusicPlayer.ThisPlayerIsDone();
                            break;
                        case MusicTransitionType.CrossFade:
                            currentMusicPlayer.FullFadeOut(musicSwitchData.crossFadeTime);
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState, 0, 0,
                                musicSwitchData.crossFadeTime);
                            break;
                        case MusicTransitionType.WaitForCurrentPartToFinish:
                            currentMusicPlayer.DoneAfterThisPart();
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState,
                                currentMusicPlayer.GetCurrentPartPlayingTime());
                            break;
                        case MusicTransitionType.WaitForWholeTrackToFinish:
                            currentMusicPlayer.DoneAfterWholeTrack();
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState,
                                currentMusicPlayer.GetSecondsLeftForWholeTrackToFinish());
                            break;
                        default:
                            Debug.LogError("This transition type is not implemented yet");
                            CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState);
                            currentMusicPlayer.ThisPlayerIsDone();
                            break;
                    }
                }
            }
            else
            {
                var currentMusicPlayer = _musicPlayers.Last();
                //Fade out all music players and add new music player 
                foreach (var mp in _musicPlayers)
                {
                    if (currentMusicPlayer == mp) continue;
                    mp.FullFadeOut(musicSwitchData.crossFadeTime);
                }
                
                switch (musicSwitchData.transitionType)
                {
                    case MusicTransitionType.Cut:
                        CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState);
                        currentMusicPlayer.ThisPlayerIsDone();
                        break;
                    case MusicTransitionType.CrossFade:
                        currentMusicPlayer.FullFadeOut(musicSwitchData.crossFadeTime);
                        CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState, 0, 0,
                            musicSwitchData.crossFadeTime);
                        break;
                    case MusicTransitionType.WaitForCurrentPartToFinish:
                        currentMusicPlayer.DoneAfterThisPart();
                        CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState,
                            currentMusicPlayer.GetCurrentPartPlayingTime());
                        break;
                    case MusicTransitionType.WaitForWholeTrackToFinish:
                        currentMusicPlayer.DoneAfterWholeTrack();
                        CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState,
                            currentMusicPlayer.GetSecondsLeftForWholeTrackToFinish());
                        break;
                    default:
                        Debug.LogError("This transition type is not implemented yet");
                        CreateNewMusicPlayer(musicSwitchData.newMusicTrack, musicSwitchData.startFromState);
                        currentMusicPlayer.ThisPlayerIsDone();
                        break;
                }
                
            }
            
            
            
        }

        private MusicPlayer CreateNewMusicPlayer(MusicTrack musicTrack, MusicState startingState,
            double playingDelay = 0, double startTime = 0, float fadeInTime = 0)
        {
            MusicPlayer newMusicPlayer = Instantiate(musicPlayerPrefab, transform);
            newMusicPlayer.transform.parent = transform;
            newMusicPlayer.Init(musicTrack, startingState, playingDelay, startTime, fadeInTime);
            _musicPlayers.Add(newMusicPlayer);
            return newMusicPlayer;
        }


        public void RemoveMusicPlayerFromQueue(MusicPlayer musicPlayer)
        {
            _musicPlayers.Remove(musicPlayer);
        }
    }
}