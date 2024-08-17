using UnityEngine;

namespace Player
{
    public class PlayerBootstrap : MonoBehaviour
    {
        [SerializeField] private Spear.Spear spear;
        [SerializeField] private Player player;
        
        private void Awake()
        {
            spear.Init();
        }
    }
}