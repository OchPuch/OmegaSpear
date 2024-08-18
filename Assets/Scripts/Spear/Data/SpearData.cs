using System;
using UnityEngine;

namespace Spear.Data
{
    [Serializable]
    public class SpearData
    {
        public float desiredRotation;
        [field: SerializeField] public SpearConfig SpearConfig { get; private set; }
        [field: SerializeField] public Player.Player Player { get; private set; }
        [field: SerializeField] public SpearScaler SpearScaler { get; private set; }
        [field: SerializeField] public Rigidbody CenterRigidbody { get; private set; }
        public Transform CenterTransform => SpearScaler.CenterPoint;
        public bool ExpandRequest { get; private set; }
        public bool ShrinkRequest { get; private set; }
        
        public bool WasExpandRequest { get; private set; }
        public bool WasShrinkRequest { get; private set; }
        
        public float Scale => SpearScaler.GetScale();


        public float loadTimer;


        public void UpdateInput()
        {
            ExpandRequest = Input.GetMouseButton(0);
            ShrinkRequest = Input.GetMouseButton(1);
        }

        public void SaveLastInput()
        {
            WasExpandRequest = ExpandRequest;
            WasShrinkRequest = ShrinkRequest;
        }
    }

    
}