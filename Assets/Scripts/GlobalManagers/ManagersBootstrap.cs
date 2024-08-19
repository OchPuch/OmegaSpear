using UnityEngine;

namespace GlobalManagers
{
    public class ManagersBootstrap : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private EnvironmentObjectsManager environmentObjectsManager;

        private void Awake()
        {
            pauseManager.Init();
            timeManager.Init();
        }
    }
}
