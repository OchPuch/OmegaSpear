using UnityEngine;

namespace GlobalManagers
{
    public class ManagersBootstrap : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private EnvironmentObjectsManager environmentObjectsManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private MusicManager musicManager;
        [SerializeField] private CheckpointManager checkpointManager;

        private void Awake()
        {
            pauseManager.Init();
            timeManager.Init();
            environmentObjectsManager.Init();
            levelManager.Init();
            musicManager.Init();
            checkpointManager.Init();
        }
    }
}
