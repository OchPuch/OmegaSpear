using System;
using Music;
using UnityEngine;
using Utils;

namespace Levels
{
    [CreateAssetMenu(menuName = "OMEGA_SPEAR/Level")]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public Sprite PreviewImage { get; private set; }
        [field: SerializeField] public Sprite PreviewSecretImage { get; private set; }
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public SceneField Scene { get; private set; }
        [field: SerializeField] public MusicSwitchData MusicSwitchData { get; private set;} 
        [field: Header("Time")]
        [field: SerializeField] public float Ctime { get; private set; }
        [field: SerializeField] public float Btime { get; private set; }
        [field: SerializeField] public float Atime { get; private set; }
        [field: SerializeField] public float Stime { get; private set; }
        
        [field: Header("Other Levels")]
        [field: SerializeField] public LevelData[] UnlockLevels { get; private set; }
        
        private int _levelId;
        public int LevelId
        {
            get
            {
                if (_levelId == 0)
                {
                    Debug.Log("Level id set");
                    _levelId = GetInstanceID();
                }

                return _levelId;
            }
        }
        
        //TODO: KYS
        private string LevelStatusKey => Scene.SceneName + name + "_level_status";
        private string LevelHoursKey => Scene.SceneName + name + "_level_hours_time";
        private string LevelMinutesKey => Scene.SceneName + name + "_level_minutes_time";
        private string LevelSecondsKey => Scene.SceneName + name + "_level_seconds_time";
        public LevelStatus LevelStatus => (LevelStatus)PlayerPrefs.GetInt(LevelStatusKey, 0);

        private void SetLevelStatus(LevelStatus levelStatus)
        {
            PlayerPrefs.SetInt(LevelStatusKey, (int) levelStatus);
        }
        
        public void Unlock()
        {
            if (LevelStatus is LevelStatus.Locked)
            {
                SetLevelStatus(LevelStatus.Unlocked);
            }
        }

        public LevelRank GetRank()
        {
            if (LevelStatus is LevelStatus.Locked or LevelStatus.Unlocked)
                return  LevelRank.None;
            if (LevelStatus is LevelStatus.Perfect)
                return LevelRank.P;
            
            float time = GetTime();
            if (time < Stime) return LevelRank.S;
            if (time < Atime) return LevelRank.A;
            if (time < Btime) return LevelRank.B;
            if (time < Ctime) return LevelRank.C;
            return LevelRank.D;
        }

        public void Complete(int hh, int mm, float ss, bool usedCheckpoints)
        {
            if (LevelStatus is not LevelStatus.Perfect)
            {
                SetLevelStatus(LevelStatus.Completed);
            }

            float time = TimeUtils.GetTime(hh, mm, ss);
            if (time < Stime && !usedCheckpoints)
            {
                if (LevelStatus is not LevelStatus.Perfect)
                {
                    SetTime(hh,mm,ss);
                }
                else
                {
                    if (time < GetTime())
                    {
                        SetTime(hh,mm,ss);
                    }
                }
                SetLevelStatus(LevelStatus.Perfect);
            }
            else if (LevelStatus is not LevelStatus.Perfect)
            {
                if (time < GetTime() || GetTime() == 0)
                {
                    SetTime(hh,mm,ss);
                }
            }

            
            
        }

        public string GetFormattedTime()
        {
            if (LevelStatus is LevelStatus.Unlocked or LevelStatus.Locked)
                return "--:--:--.---";
            var hh = PlayerPrefs.GetInt(LevelHoursKey, 0);
            var mm = PlayerPrefs.GetInt(LevelMinutesKey, 0);
            var ss = PlayerPrefs.GetFloat(LevelSecondsKey, 0);
            return TimeUtils.GetFormattedTime(hh, mm, ss);
        }

        public float GetTime()
        {
            var hh = PlayerPrefs.GetInt(LevelHoursKey, 0);
            var mm = PlayerPrefs.GetInt(LevelMinutesKey, 0);
            var ss = PlayerPrefs.GetFloat(LevelSecondsKey, 0);
            return TimeUtils.GetTime(hh, mm, ss);
        }
        
        private void SetTime(int hh, int mm, float ss)
        {
            PlayerPrefs.SetInt(LevelHoursKey, hh);
            PlayerPrefs.SetInt(LevelMinutesKey, mm);
            PlayerPrefs.SetFloat(LevelSecondsKey, (float) Math.Round(ss, 3, MidpointRounding.AwayFromZero));
        }
    }
}