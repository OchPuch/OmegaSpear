using System.Collections;
using GlobalManagers;
using UnityEngine;
using Utils;

namespace Music
{
    public class MusicPlayer : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private AudioSource mainSource;

        [SerializeField] private AudioSource introSource;
        [SerializeField] private AudioSource outroSource;

        private MusicTrack _currentTrack;
        private MusicState _currentState;

        private Coroutine _musicStateUpdaterCoroutine;

        private double _playingDelay;
        private double _creationTime;
        
        private Coroutine _fadeOutCoroutine;
        private Coroutine _fadeInCoroutine;
        private Coroutine _crossFadeCoroutine;

        public void Init(MusicTrack musicTrack, MusicState startingState, double playingDelay = 0, double startTime = 0, double fadeInTime = 0)
        {
            _creationTime = AudioSettings.dspTime;

            _currentTrack = musicTrack;
            _playingDelay = playingDelay;
            if (_playingDelay > 0) _currentState = MusicState.Preparing;

            StartNewMusicTrack(startingState, startTime);
            if (fadeInTime > 0) _fadeInCoroutine = StartCoroutine(FadeIn(fadeInTime));
        }

        private bool IsMusicPlaying()
        {
            return introSource.isPlaying || mainSource.isPlaying || outroSource.isPlaying;
        }

        private void StartNewMusicTrack(MusicState newMusicState, double startTime)
        {
            _currentState = newMusicState;
            switch (newMusicState)
            {
                case MusicState.Intro:
                    introSource.clip = _currentTrack.intro;
                    introSource.time = (float)startTime;
                    introSource.PlayScheduled(AudioSettings.dspTime + _playingDelay);
                    mainSource.loop = _currentTrack.loopMain;
                    mainSource.clip = _currentTrack.main;
                    mainSource.PlayScheduled(AudioSettings.dspTime + _playingDelay +
                                             GetSecondsLeftForCurrentPartToFinish(introSource));
                    break;
                case MusicState.Main:
                    mainSource.clip = _currentTrack.main;
                    mainSource.loop = _currentTrack.loopMain;
                    mainSource.time = (float)startTime;
                    mainSource.PlayScheduled(AudioSettings.dspTime + _playingDelay);
                    break;
                case MusicState.Outro:
                    outroSource.clip = _currentTrack.outro;
                    outroSource.time = (float)startTime;
                    outroSource.PlayScheduled(AudioSettings.dspTime + _playingDelay);
                    break;
            }

            if (_musicStateUpdaterCoroutine != null) StopCoroutine(_musicStateUpdaterCoroutine);
            _musicStateUpdaterCoroutine = StartCoroutine(MusicStateUpdater());
        }

        private IEnumerator MusicStateUpdater()
        {
            yield return new WaitForSecondsRealtime((float)_playingDelay);
            yield return null;
            if (!IsMusicPlaying())
            {
                yield break;
            }

            while (true)
            {
                double secondsLeft = 0;
                if (introSource.isPlaying)
                {
                    secondsLeft = GetSecondsLeftForCurrentPartToFinish(introSource);
                    _currentState = MusicState.Intro;
                }
                else if (mainSource.isPlaying)
                {
                    secondsLeft = GetSecondsLeftForCurrentPartToFinish(mainSource);
                    _currentState = MusicState.Main;
                }
                else if (outroSource.isPlaying)
                {
                    secondsLeft = GetSecondsLeftForCurrentPartToFinish(outroSource);
                    _currentState = MusicState.Outro;
                }
                else
                {
                    ThisPlayerIsDone();
                    yield break;
                }

                yield return new WaitForSecondsRealtime((float)secondsLeft);
            }
        }

        private IEnumerator FadeIn( double fadeInTime)
        {
            float startVolume = 0f;
            float elapsedTime = 0f;
            yield return new WaitForSeconds((float)_playingDelay);
            CurrentAudioSource().volume = 0;
            while (elapsedTime <= fadeInTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                CurrentAudioSource().volume = Mathf.Lerp(startVolume, 1, elapsedTime / (float)fadeInTime);
                yield return null;
            }
        }

        public void FullFadeOut(float time)
        {
            
            if (_fadeOutCoroutine != null)
            {
                Debug.LogWarning("Fade out coroutine is already running");
                return;
            }

            if (_fadeInCoroutine != null)
            {
                Debug.LogWarning("Fade in coroutine is already running");
                StopCoroutine(_fadeInCoroutine);
                _fadeInCoroutine = null;
            }
            
            
            _fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(time));
        }
        
        private void CrossFadeBetweenParts(AudioSource from, AudioSource to, float time)
        {
            if (_fadeOutCoroutine != null)
            {
                Debug.LogWarning("Fade out coroutine is already running");
                return;
            }
            if (_fadeInCoroutine != null)
            {
                StopCoroutine(_fadeInCoroutine);
                _fadeInCoroutine = null;
            }
            _crossFadeCoroutine = StartCoroutine(CrossFadeBetweenPartsCoroutine(from, to, time));
        }
        
        private IEnumerator CrossFadeBetweenPartsCoroutine(AudioSource from, AudioSource to, float time)
        {
            float startVolume = from.volume;
            float elapsedTime = 0f;
            while (elapsedTime <= time)
            {
                elapsedTime += Time.unscaledDeltaTime;
                from.volume = Mathf.Lerp(startVolume, 0, elapsedTime / time);
                to.volume = Mathf.Lerp(0, 1, elapsedTime / time);
                yield return null;
            }
            
            _crossFadeCoroutine = null;
        }
        
        private IEnumerator FadeOutCoroutine(float time)
        {
            float startVolume = CurrentAudioSource().volume;
            float elapsedTime = 0f;
            while (elapsedTime <= time)
            {
                elapsedTime += Time.unscaledDeltaTime;
                introSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / time);
                mainSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / time);
                outroSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / time);
                yield return null;
            }
            
            _fadeOutCoroutine = null;
            ThisPlayerIsDone();
        }

        private double GetSecondsLeftForCurrentPartToFinish(AudioSource audioSource)
        {
            switch (_currentState)
            {
                case MusicState.Intro:
                    return SoundUtils.GetClipLength(audioSource.clip) - audioSource.time;
                case MusicState.Main:
                    return SoundUtils.GetClipLength(audioSource.clip) - audioSource.time;
                case MusicState.Outro:
                    return SoundUtils.GetClipLength(audioSource.clip) - audioSource.time;
                default:
                    return 0;
            }
        }

        private AudioSource CurrentAudioSource()
        {
            switch (_currentState)
            {
                case MusicState.Intro:
                    return introSource;
                case MusicState.Main:
                    return mainSource;
                case MusicState.Outro:
                    return outroSource;
                default:
                    return null;
            }
        }
        
        private AudioSource GetAudioSource(MusicState state)
        {
            switch (state)
            {
                case MusicState.Intro:
                    return introSource;
                case MusicState.Main:
                    return mainSource;
                case MusicState.Outro:
                    return outroSource;
                default:
                    return null;
            }
        }
        

        public double GetSecondsLeftForWholeTrackToFinish()
        {
            switch (_currentState)
            {
                case MusicState.Preparing:
                    return AudioSettings.dspTime - _creationTime + _playingDelay +
                           SoundUtils.GetClipLength(_currentTrack.intro) +
                           SoundUtils.GetClipLength(_currentTrack.main) + SoundUtils.GetClipLength(_currentTrack.outro);
                case MusicState.Intro:
                    return SoundUtils.GetClipLength(CurrentAudioSource().clip) - CurrentAudioSource().time +
                           SoundUtils.GetClipLength(_currentTrack.main) +
                           SoundUtils.GetClipLength(_currentTrack.outro);
                case MusicState.Main:
                    return SoundUtils.GetClipLength(CurrentAudioSource().clip) - CurrentAudioSource().time +
                           SoundUtils.GetClipLength(_currentTrack.outro);
                case MusicState.Outro:
                    return SoundUtils.GetClipLength(CurrentAudioSource().clip) - CurrentAudioSource().time;
                default:
                    return 0;
            }
        }

        public double GetCurrentPartPlayingTime()
        {
            return CurrentAudioSource() is null ? 0f : CurrentAudioSource().time;
        }
        
        public MusicState GetCurrentState()
        {
            return _currentState;
        }
        
        public MusicTrack GetMusicTrack()
        {
            return _currentTrack;
        }
        
        //YOU ARE DONE
        //FIRED
        //Do not show your face at the laundry again
        //Stay away from Pinkman
        //Do NOT go near him
        //EVER
        //Are you listening to me?
        public void ThisPlayerIsDone()
        {
            MusicManager.Instance.RemoveMusicPlayerFromQueue(this);
            _currentState = MusicState.Done;
            introSource.Stop();
            mainSource.Stop();
            outroSource.Stop();
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void DoneAfterThisPart()
        {
            StartCoroutine(DeathSentence(GetSecondsLeftForCurrentPartToFinish(CurrentAudioSource())));
        }
        
        public void DoneAfterWholeTrack()
        {
            StartCoroutine(DeathSentence(GetSecondsLeftForWholeTrackToFinish()));
        }
        
        private IEnumerator DeathSentence(double time)
        {
            yield return new WaitForSecondsRealtime((float)time);
            ThisPlayerIsDone();
        }


        public void ChangeMusicState(MusicState startFromState, MusicTransitionType transitionType, float crossFadeTime = 0)
        {
            if (_currentState == startFromState) return;
            if (_currentState == MusicState.Preparing)
            {
                StartNewMusicTrack(startFromState, 0);
                return;
            }
            
            double delayTime = 0;
            if (_currentState == MusicState.Main) mainSource.loop = false;

            switch (transitionType)
            {
                case MusicTransitionType.Cut:
                    CurrentAudioSource().Stop();
                    delayTime = 0;
                    break;
                case MusicTransitionType.CrossFade:
                    CrossFadeBetweenParts(GetAudioSource(_currentState), GetAudioSource(startFromState), crossFadeTime);
                    delayTime = 0;
                    break;
                case MusicTransitionType.WaitForCurrentPartToFinish:
                    delayTime = (GetSecondsLeftForCurrentPartToFinish(CurrentAudioSource()));
                    break;
                case MusicTransitionType.WaitForWholeTrackToFinish:
                    delayTime = (GetSecondsLeftForWholeTrackToFinish());
                    break;
                default:
                    Debug.LogError("" + transitionType + " transition is not implemented yet");
                    break;
            }
            
            switch (startFromState)
            {
                case MusicState.Intro:
                    introSource.clip = _currentTrack.intro;
                    introSource.time = 0;
                    introSource.PlayScheduled(AudioSettings.dspTime + delayTime);
                    mainSource.loop = _currentTrack.loopMain;
                    mainSource.clip = _currentTrack.main;
                    mainSource.PlayScheduled(AudioSettings.dspTime + delayTime +
                                             SoundUtils.GetClipLength(_currentTrack.intro));
                    break;
                case MusicState.Main:
                    mainSource.clip = _currentTrack.main;
                    mainSource.loop = _currentTrack.loopMain;
                    mainSource.time = 0;
                    mainSource.PlayScheduled(AudioSettings.dspTime + delayTime);
                    break;
                case MusicState.Outro:
                    outroSource.clip = _currentTrack.outro;
                    outroSource.time = 0;
                    outroSource.PlayScheduled(AudioSettings.dspTime + delayTime);
                    break;
                default:
                    Debug.LogError("" + startFromState + " state is not implemented yet");
                    break;
            }

            if (_musicStateUpdaterCoroutine != null) StopCoroutine(_musicStateUpdaterCoroutine);
            _musicStateUpdaterCoroutine = StartCoroutine(MusicStateUpdater());
        }
        
       
    }
}