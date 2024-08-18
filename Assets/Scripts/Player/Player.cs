using CommonObjects;
using Player.Data;

namespace Player
{
    public class Player : GamePlayBehaviour
    {
        public IPlayerEvents PlayerEvents => _data.PlayerEvents;
        private PlayerData _data;
        public PlayerData PlayerData => _data;
        
        public void Init(PlayerData playerData)
        {
            _data = playerData;
            PlayerEvents.Died += OnDeath;
        }

        private void OnDeath()
        {
            gameObject.SetActive(false);
        }
    }
}