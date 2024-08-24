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
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private CheckpointManager checkpointManager;

        private void Awake()
        {
            if (pauseManager) pauseManager.Init();
            if (timeManager) timeManager.Init();
            if (environmentObjectsManager) environmentObjectsManager.Init();
            if (levelManager) levelManager.Init();
            if (soundManager) soundManager.Init();
            if (musicManager) musicManager.Init();
            if (checkpointManager) checkpointManager.Init();
        }
    }
}
