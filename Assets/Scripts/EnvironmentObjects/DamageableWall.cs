using CommonObjects;
using UnityEngine;

namespace EnvironmentObjects
{
    public class DamageableWall : GamePlayBehaviour, IDamageable
    {
        [SerializeField] private ParticleSystem deathParticles;

        public void Damage(float damage)
        {
            
        }
    }
}