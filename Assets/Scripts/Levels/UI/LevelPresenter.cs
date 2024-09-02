using System;
using GlobalManagers;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.UI
{
    public class LevelPresenter : MonoBehaviour
    {
        [Header("Info Presenters")] [SerializeField]
        private TextMeshProUGUI levelName;
        [SerializeField] private Image levelPreview;
        [SerializeField] private TextMeshProUGUI personalBestTime;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image backgroundPanel;
        [SerializeField] private Button startButton;
        [Header("Perfected")] 
        [SerializeField] private Color perfectBackgroundColor;
        [SerializeField] private Color perfectBaseTextColor;
        [SerializeField] private Color perfectExtraTextColor;
        [Header("Rank")]
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private Image backgroundRank;
        [SerializeField] private Color pColor;
        [SerializeField] private Color sColor;
        [SerializeField] private Color aColor;
        [SerializeField] private Color bColor;
        [SerializeField] private Color cColor;
        [SerializeField] private Color dColor;


        private LevelData _data;

        public void Init(LevelData data)
        {
            _data = data;
            levelName.text = _data.LevelName;
            if (_data.LevelStatus is not LevelStatus.Locked)
            {
                if (_data.PreviewImage) levelPreview.sprite = _data.PreviewImage;
            }

            rankText.text = _data.GetRank().ToString();
            switch (_data.GetRank())
            {
                case LevelRank.D:
                    backgroundRank.color = dColor;
                    break;
                case LevelRank.C:
                    backgroundRank.color = cColor;
                    break;
                case LevelRank.B:
                    backgroundRank.color = bColor;
                    break;
                case LevelRank.A:
                    backgroundRank.color = aColor;
                    break;
                case LevelRank.S:
                    backgroundRank.color = sColor;
                    break;
                case LevelRank.P:
                    backgroundRank.color = pColor;
                    break;
                case LevelRank.None:
                    rankText.text = "N/A";
                    break;
            }


            switch (_data.LevelStatus)
            {
                case LevelStatus.Locked:
                    statusText.text = "LOCKED";
                    startButton.interactable = false;
                    break;
                case LevelStatus.Unlocked:
                    statusText.text = "PLAY";
                    break;
                case LevelStatus.Completed:
                    statusText.text = "COMPLETED";
                    personalBestTime.text = _data.GetFormattedTime();
                    break;
                case LevelStatus.Perfect:
                    statusText.text = "PERFECT";
                    personalBestTime.text = _data.GetFormattedTime();
                    personalBestTime.color = perfectExtraTextColor;
                    levelName.color = perfectBaseTextColor;
                    statusText.color = perfectBaseTextColor;
                    backgroundPanel.color = perfectBackgroundColor;
                    //TODO: ADD SECRET IMAGE
                    if (_data.PreviewSecretImage) levelPreview.sprite = _data.PreviewSecretImage;
                    break;
            }
        }


        public void LoadThisLevel()
        {
            MusicManager.Instance.SwitchMusic(_data.MusicSwitchData);
            CheckpointManager.Instance.DeleteCheckpoint();
            LevelManager.Instance.LoadNewLevel(_data.Scene);
        }
    }
}