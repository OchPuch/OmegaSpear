using CommonObjects;
using Player.Data;

namespace Player
{
    public class PlayerHealth : GamePlayBehaviour
    {
        public int Health { get; private set; }
        
        private PlayerData _data;
        
        public void Init(PlayerData playerData)
        {
            _data = playerData;
            Health = _data.Config.MaxHealth;
        }

        public void Heal()
        {
            Health = _data.Config.MaxHealth;
        }

        public void Damage(int damage)
        {
            if (damage < 0 ) return;
            Health -= damage;
            if (Health <= 0)
            {
                _data.PlayerEvents.InvokeDeath();
            }
        }
    }
}