using System;
using UnityEngine;

namespace Spear.Data
{
    [Serializable]
    public class SpearData
    {
        [field: SerializeField] public SpearConfig SpearConfig { get; private set; }
        
        public float desiredRotation;
    }

    
}