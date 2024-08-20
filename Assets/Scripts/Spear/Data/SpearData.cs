using System;
using UnityEngine;

namespace Spear.Data
{
    [Serializable]
    public class SpearData
    {
        public bool poor;
        public float desiredRotation;
        [field: SerializeField] public SpearConfig SpearConfig { get; private set; }
        [field: SerializeField] public Player.Player Player { get; private set; }
        [field: SerializeField] public SpearScaler SpearScaler { get; private set; }
        [field: SerializeField] public TipPoint TipPoint { get; private set; }
        [field: SerializeField] public Rigidbody CenterRigidbody { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        public Transform CenterTransform => SpearScaler.CenterPoint;
        public bool ExpandRequest { get; private set; }
        public bool ShrinkRequest { get; private set; }

        public bool WasExpandRequest { get; private set; }
        public bool WasShrinkRequest { get; private set; }

        public float Scale => SpearScaler.GetScale();


        public float UmbrellaCharge { get; private set; }
        public float loadTimer;

        public void Init()
        {
            SpearScaler.Init(this);
            TipPoint.Init(this);
        }

        public void UpdateInput()
        {
            ExpandRequest = Input.GetMouseButton(0);
            ShrinkRequest = Input.GetMouseButton(1);
        }

        public float GetDamage()
        {
            var velocityProject=  Vector3.Project(Player.PlayerData.ControlledCollider.GetVelocity(), SpearScaler.HandlePoint.right);
            return Mathf.Clamp(velocityProject.magnitude * SpearConfig.DamageBySpeedMultiplier, 0,
                SpearConfig.MaxDamage); 
        }

        public void AddUmbrellaCharge(float add)
        {
            UmbrellaCharge += add;
            UmbrellaCharge = Mathf.Clamp(UmbrellaCharge, 0, SpearConfig.UmbrellaMaxTimeCharge);
        }

        public void SaveLastInput()
        {
            WasExpandRequest = ExpandRequest;
            WasShrinkRequest = ShrinkRequest;
        }
    }
}