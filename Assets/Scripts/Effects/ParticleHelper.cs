using CartoonFX;
using UnityEngine;
using UnityEngine.Pool;

namespace Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleHelper : MonoBehaviour
    {
        private ObjectPool<ParticleSystem> _pool;
        private ParticleSystem _particleSystem;
        private CFXR_Effect _effect;
        private Vector3 _baseShakeStrength;
        public void SetPool(ObjectPool<ParticleSystem> pool)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _effect = GetComponent<CFXR_Effect>();
            if (_effect)
            {
                _effect.clearBehavior = CFXR_Effect.ClearBehavior.None;
                _baseShakeStrength = _effect.cameraShake.shakeStrength;
            }
            
            _pool = pool;
            
        }

        private void Update()
        {
            if (_effect is null) return;
            _effect.cameraShake.shakeStrength = _baseShakeStrength * Time.timeScale;
        } 

        private void OnParticleSystemStopped()
        {
            _pool.Release(_particleSystem);
        }
    }
}