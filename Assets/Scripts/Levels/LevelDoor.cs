using GlobalManagers;
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
        private bool _entered;
        private void OnTriggerEnter(Collider other)
        { 
            if (_entered) return;
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

            _entered = true;
        }
    }
}