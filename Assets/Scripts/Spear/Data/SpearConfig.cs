using System;
using Spear.States.General;
using UnityEngine;

namespace Spear.Data
{
    [CreateAssetMenu(menuName = "OMEGA_SPEAR/Spear", fileName = "Spear Config")]
    public class SpearConfig : ScriptableObject
    {
        [field: Header("General")]
        [field: SerializeField] public float DamageBySpeedMultiplier { get; private set; }
        [field: SerializeField] public float MaxDamage { get; private set; }
        [field: SerializeField] public LayerMask HitMask { get; private set; }
        [field: SerializeField] public LayerMask LockMask { get; private set; }
        [field: SerializeField] public LayerMask HardGroundMask { get; private set; }
        [field: SerializeField] public float ImpulsePlayerSpeed { get; private set; } 
        [field: SerializeField] public float ImpulseRadius { get; private set; }
        [field: SerializeField] public AnimationCurve ImpulseScaleByCharge { get; private set; }
        [field: SerializeField] public float MaxImpulseChargeTime { get; private set; }
        [field: SerializeField] public float ShrinkHoldTimeToUnlock { get; private set; }
        [field: SerializeField] public float UnstuckFromGroundScale { get; private set; }
        [field: SerializeField] public float StuckShrinkSpeedMultiplier { get; private set; }
        [field: SerializeField] public float StuckExpandSpeedMultiplier { get; private set; }
        [field: SerializeField] public float ExtraStuckSize { get; private set; }
        [field: SerializeField] public float UmbrellaDoubleJump { get; private set; }
        [field: SerializeField] public float TimeToMaxUmbrellaDoubleJump { get; private set; }
        [field: SerializeField] public float UmbrellaYSpeed { get; private set; }
        
        [field: SerializeField] public float UmbrellaMaxTimeCharge { get; private set; } = 2f;
        
        [field: SerializeField] public AnimationCurve UmbrellaYSpeedByCharge { get; private set; }
        
        [field: SerializeField] public AnimationCurve UmbrellaDoubleJumpByCharge { get; private set; }
        [field: Header("Visuals")]
        [field: SerializeField] public AnimationCurve ShakeIntro { get; private set; }
        [field: SerializeField] public AnimationCurve ShakeLoop { get; private set; }
        [field: Header("Settings")]
        [field: SerializeField] public SpearStateSettings NormalSettings { get; private set; }
        [field: SerializeField] public SpearStateSettings OnceLoadedSettings { get; private set; }
        [field: SerializeField] public SpearStateSettings TwiceLoadedSettings { get; private set; }
        [field: SerializeField] public SpearStateSettings UmbrellaSettings { get; private set; }
        [field: SerializeField] public SpearStateSettings FromAnyToUmbrellaSettings { get; private set; }

        [field: Header("Transitions")]
        [field: SerializeField] public TransitionSettings ImpulseTransition { get; private set; }
        [field: SerializeField] public TransitionSettings UltraExtendTransition { get; private set; }
        [field: SerializeField] public TransitionSettings FromUmbrellaToNormalTransition { get; private set; }
        
    }
    
    [Serializable]
    public class SpearStateSettings
    {
        [field: Header("Scaling")]
        [field: SerializeField] public float ScalingMultiplier { get; private set; } = 2f;
        [field: SerializeField] public AnimationCurve ExpandingSpeedByHolding { get; private set; }
        
        [field: SerializeField] public AnimationCurve FatingSpeedWhileExpanding { get; private set; }
        [field: SerializeField] public AnimationCurve ShrinkingSpeedByHolding { get; private set; }
        
        [field: SerializeField] public AnimationCurve FatingSpeedWhileShrinking { get; private set; }
        [field: SerializeField] public float MaxExpand { get; private set; }
        [field: SerializeField] public float MinShrink { get; private set; }
        [field: SerializeField] public float MinScaleToChangeState { get; private set; }

        [field: Header("Rotation")]
        [field: SerializeField] public float RotationDamping { get; private set; }
        
        [field: Header("Effects")]
        [field: SerializeField] public ParticleSystem StopEffect { get; private set; }
        [field: SerializeField] public AudioClip SpecialSound1 { get; private set; }
        [field: SerializeField] public AudioClip SpecialSound2 { get; private set; }
    }

    [Serializable]
    public class TransitionSettings
    {
        [field: SerializeField] public float RotationDamping { get; private set; } = 45f;
        [field: SerializeField] public AnimationCurve Transition { get; private set; }
        [field: SerializeField] public AnimationCurve FatingWileTransition { get; private set; }
        [field: SerializeField] public float SpecialActionTime { get; private set; }
        [field: SerializeField] public ParticleSystem SpecialEffect { get; private set; }
        [field: SerializeField] public AudioClip SpecialSound { get; private set; }
        [field: SerializeField] public AudioClip SpecialSound2 { get; private set; }

        [field: SerializeField] public float MaxExpand { get; private set; }
        [field: SerializeField] public float MinShrink { get; private set; }
    }
}