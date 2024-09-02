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
        private bool _entered;
        private void OnTriggerEnter(Collider other)
        { 
            if (_entered) return;
            TimeUtils.SplitTime(TimeManager.Instance.LevelTimer, out var hh, out var mm, out var ss);
            completedLevel.Complete(hh,mm,ss, LevelManager.Instance.Reloads > 0);
            if (completedLevel.UnlockLevels.Length > 0)
            {
                foreach (var unlockLevel in completedLevel.UnlockLevels)
                {
                    unlockLevel.Unlock();
                }
                LevelManager.Instance.LoadNewLevel(completedLevel.UnlockLevels[0].Scene);
            }
            else
            {
                SceneManager.LoadScene(mainMenu);
            }

            _entered = true;
        }
    }
}