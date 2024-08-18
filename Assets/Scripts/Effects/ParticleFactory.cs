using UnityEngine;
using UnityEngine.Pool;

namespace Effects
{
    public class ParticleFactory
    {
        private readonly ParticleSystem _effect;
        private readonly ObjectPool<ParticleSystem> _pool;
        
        public ParticleFactory(ParticleSystem effect)
        {
            _effect = effect;
            _pool = new ObjectPool<ParticleSystem>(CreateNewParticleSystem, OnTake, OnReturn, OnDestroyParticleSystem, true, 1, 5);
        }

        private void OnDestroyParticleSystem(ParticleSystem obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void OnReturn(ParticleSystem obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnTake(ParticleSystem obj)
        {
            obj.gameObject.SetActive(true);
        }

        private ParticleSystem CreateNewParticleSystem()
        {
            if (_effect is null) return null;
            var effect = Object.Instantiate(_effect);
            effect.gameObject.SetActive(false);
            effect.GetComponent<ParticleHelper>().SetPool(_pool);
            return effect;
        }

        public ParticleSystem CreateParticleSystem(Vector2 position)
        {
            var effect = _pool.Get();
            effect.transform.position = position;
            return effect;
        }
        
    }
}