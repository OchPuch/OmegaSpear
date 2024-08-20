using UnityEngine;

namespace Music
{
    [CreateAssetMenu(fileName = "MusicTrack", menuName = "Music/MusicTrack", order = 1)]
    public class MusicTrack : ScriptableObject
    {
        [Header("Clips")]
        [Tooltip("Recommended to make intro really short")]
        public AudioClip intro;
        public AudioClip main;
        [Tooltip("Recommended to make outro really short")]
        public AudioClip outro;
        
        [Header("Looping")]
        public bool loopMain;
        
        public AudioClip GetClip(MusicState state)
        {
            switch (state)
            {
                case MusicState.Intro:
                    return intro;
                case MusicState.Main:
                    return main;
                case MusicState.Outro:
                    return outro;
                default:
                    return null;
            }
        }

    }
}