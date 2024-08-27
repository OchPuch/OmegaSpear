using System;
using GlobalManagers;
using TMPro;
using UnityEngine;
using Utils;

namespace GeneralUI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer;
        private void Update()
        {
            if (TimeManager.Instance)
            {
                timer.text = TimeUtils.GetFormattedTime(TimeManager.Instance.LevelTimer);
                
            }
        }
    }
}