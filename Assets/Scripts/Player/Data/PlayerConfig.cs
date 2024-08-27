using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(menuName = "OMEGA_SPEAR/Player/BaseConfig", fileName = "Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        
    }
}