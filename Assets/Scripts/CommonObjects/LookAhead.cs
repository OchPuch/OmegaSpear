using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CommonObjects
{
    public class LookAhead : GamePlayBehaviour
    {
        [SerializeField] private ControlledCollider controlledCollider;
        [SerializeField] private float distanceMultiplier = 1f;
        [SerializeField] private bool ignoreY = true;
        [SerializeField] private bool useMinSpeed;
        [ShowIf("useMinSpeed")] 
        [SerializeField] private float minSpeed;

        private void FixedUpdate()
        {
            transform.localPosition = controlledCollider.GetVelocity() * distanceMultiplier;
            if (ignoreY)
            {
                var vector3 = transform.localPosition;
                vector3.y = 0f;
                transform.localPosition = vector3;
            }
            if (useMinSpeed)
            {
                if (controlledCollider.GetVelocity().magnitude < minSpeed)
                {
                    transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}