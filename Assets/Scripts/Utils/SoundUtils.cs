using UnityEngine;

namespace Utils
{
    public class SoundUtils
    {
        public static double GetClipLength(AudioClip clip)
        {
            return clip is null? 0 : (double)clip.samples / clip.frequency;
        }
    }
}