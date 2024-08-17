using System;
using UnityEngine;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        [field: SerializeField] public PlayerConfig Config { get; private set; }

    }
}