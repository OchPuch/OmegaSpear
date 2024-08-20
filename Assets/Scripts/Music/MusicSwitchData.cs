using System;
using UnityEngine;

namespace Music
{
    [CreateAssetMenu(fileName = "MusicTriggerData", menuName = "Music/MusicTriggerData", order = 1)]
    public class MusicSwitchData : ScriptableObject
    {
        [Header("Music settings")]
        public MusicTrack newMusicTrack;
        public MusicState startFromState;
        [Header("Transition settings")]
        [Tooltip("Turning this on will ignore \"Start from state\" setting Use if you sure both tracks have same length and you want to synchronize the Working only with \"Cut\" or \"CrossFade\" transition types")]
        public bool synchronizeWithCurrentMusic;
        [Tooltip("What should happen with current music track when this trigger is activated?")]
        public MusicTransitionType transitionType;
        [Min(0)][Tooltip("Applied if transition type is CrossFade")]
        public float crossFadeTime = 0f;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (synchronizeWithCurrentMusic)
            {
                switch (transitionType)
                {
                    case MusicTransitionType.WaitForCurrentPartToFinish:
                        transitionType = MusicTransitionType.CrossFade;
                        break;
                    case MusicTransitionType.WaitForWholeTrackToFinish:
                        transitionType = MusicTransitionType.CrossFade;
                        break;
                }
            }

            switch (transitionType)
            {
                case MusicTransitionType.WaitForCurrentPartToFinish:
                    synchronizeWithCurrentMusic = false;
                    break;
                case MusicTransitionType.WaitForWholeTrackToFinish:
                    synchronizeWithCurrentMusic = false;
                    break;
            }
        }
        
        #endif
    }
}