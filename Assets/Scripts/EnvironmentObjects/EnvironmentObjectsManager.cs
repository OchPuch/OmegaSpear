using Effects;
using UnityEngine;

namespace GlobalManagers
{
    public class EnvironmentObjectsManager : MonoBehaviour
    {
        public static EnvironmentObjectsManager Instance { get; private set; }
        public ParticleFactory WallKillParticleFactory { get; private set; }
        public ParticleFactory WallCrushParticleFactory { get; private set; }
        
        [SerializeField] private ParticleSystem wallCrushParticle;
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
            WallCrushParticleFactory = new ParticleFactory(wallCrushParticle);

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