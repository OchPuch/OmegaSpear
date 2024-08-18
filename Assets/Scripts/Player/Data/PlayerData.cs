using System;
using UnityEngine;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        [field: SerializeField] public PlayerConfig Config { get; private set; }
        [field: SerializeField] public ControlledCollider ControlledCollider { get; private set; }
        public PlayerEvents PlayerEvents { get; private set; } = new();
    }

    public class PlayerEvents : IPlayerEvents
    {
        public event Action Died;
        public event Action Respawned;
        
        public void InvokeDeath()
        {
            Died?.Invoke();
        }
        public void InvokeRespawn()
        {
            Respawned?.Invoke();
        }
    }

    public interface IPlayerEvents
    {
        public event Action Died;
        public event Action Respawned;
    }
}