using Player.Data;
using Spear.Data;
using UnityEngine;

namespace Player
{
    public class PlayerBootstrap : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Player player;
        [SerializeField] private PlayerHealth playerHealth;

        [Header("Other")] 
        [SerializeField] private SpearData spearData;
        [SerializeField] private Spear.Spear spear;
        
        private void Awake()
        {
            player.Init(playerData);
            playerHealth.Init(playerData);
            spearData.Init();
            spear.Init(spearData);
        }
    }
}