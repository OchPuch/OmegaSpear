
using System;

namespace Utils
{
    public static class TimeUtils
    {
        public static float GetTime(int hh, int mm, float ss)
        {
            return hh * 3600 + mm * 60 + ss;
        }
        
        public static void SplitTime(float seconds, out int hh, out int mm, out float ss)
        {
            hh = (int) (seconds / 3600);
            seconds -= hh * 3600;
            mm = (int)(seconds / 60);
            seconds -= mm * 60;
            ss = (float) Math.Round(seconds, 3, MidpointRounding.AwayFromZero);
        }
        
        public static string GetFormattedTime(float seconds)
        {
            int hh = (int) (seconds / 3600);
            seconds -= hh * 3600;
            int mm = (int)(seconds / 60);
            seconds -= mm * 60;
            float ss = (float) Math.Round(seconds, 3, MidpointRounding.AwayFromZero);
            return GetFormattedTime(hh, mm, ss);
        }
        
        public static string GetFormattedTime(int hh, int mm, float ss)
        {
            ss = (float) Math.Round(ss, 3, MidpointRounding.AwayFromZero);
            return $"{hh:00}:{mm:00}:{ss:00.000}";
        }
    }
}