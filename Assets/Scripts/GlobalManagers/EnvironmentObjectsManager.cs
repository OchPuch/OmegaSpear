using Effects;
using UnityEngine;

namespace GlobalManagers
{
    public class EnvironmentObjectsManager : MonoBehaviour
    {
        public static EnvironmentObjectsManager Instance { get; private set; }
        public ParticleFactory WallKillParticleFactory { get; private set; }

        [SerializeField] private ParticleSystem wallKillParticle;
        
        public void Init()
        {
            if (Instance == this) return;
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            WallKillParticleFactory = new ParticleFactory(wallKillParticle);

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

    }
}