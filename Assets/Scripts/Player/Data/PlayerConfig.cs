using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(menuName = "Player/BaseConfig", fileName = "Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int Health { get; private set; }
        
    }
}