using System;
using GlobalManagers;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelDoor : MonoBehaviour
    {
        [SerializeField] private SceneField mainMenu;
        [SerializeField] private LevelData completedLevel;
        [SerializeField] private LevelData[] unlockLevels;
        private void OnTriggerEnter(Collider other)
        { 
            TimeUtils.SplitTime(TimeManager.Instance.LevelTimer, out var hh, out var mm, out var ss);
            completedLevel.Complete(hh,mm,ss, LevelManager.Instance.Reloads > 0);
            foreach (var unlockLevel in unlockLevels)
            {
                unlockLevel.Unlock();
            }

            if (unlockLevels.Length > 0)
            {
                LevelManager.Instance.LoadNewLevel(unlockLevels[0].Scene);
            }
            else
            {
                SceneManager.LoadScene(mainMenu);
            }
        }
    }
}