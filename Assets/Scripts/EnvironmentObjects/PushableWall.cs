using System;
using CommonObjects;
using GlobalManagers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnvironmentObjects
{
    public class PushableWall : GamePlayBehaviour, ICrushable
    {
        [SerializeField] private AnimationCurve positionLerp;
        [SerializeField] private bool restorable;
        [SerializeField] private Transform lightObject;
        [SerializeField] private float restoreTime;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private bool infinitePushes;
        
        [SerializeField] private DirectionAllowed directionAllowed;
        [SerializeField] private bool fixedVelocity;
        [ShowIf("fixedVelocity")] [SerializeField] private float constVelocity;
        
        private bool _crushed;
        private bool _stopped;
        
        private float _restoreTimer;
        private Vector3 _startPosition;
        private Vector3 _stoppedPosition;
        
        
        protected override void Start()
        {
            base.Start();
            _startPosition = transform.position;
            if (lightObject != null)
            {
                if (restorable)
                {
                    lightObject.SetParent(null);
                }
                else
                {
                    lightObject.gameObject.SetActive(false);
                }
                
            }

        }

        private void Update()
        {
            if (_crushed && restorable && _stopped)
            {
                if (_restoreTimer > restoreTime)
                {
                    _restoreTimer = 0f;
                    _crushed = false;
                    _stopped = false; 
                    transform.position = _startPosition;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_crushed && restorable && _stopped)
            {
                _restoreTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(_stoppedPosition, _startPosition,
                    positionLerp.Evaluate(_restoreTimer / restoreTime));
            }
        }
        

        public void Crush(Vector2 crushDirection)
        {
            if (_crushed) return;
            rb.isKinematic = false;
            _stopped = false;
            switch (directionAllowed)
            {
                case DirectionAllowed.Any:
                    break;
                case DirectionAllowed.Right:
                    crushDirection = Vector3.Project(crushDirection, transform.right);
                    break;
                case DirectionAllowed.Up:
                    crushDirection = Vector3.Project(crushDirection, transform.up);
                    break;
            }

            if (fixedVelocity)
            {
                crushDirection = crushDirection.normalized * constVelocity;
            }
            
            rb.AddForce(crushDirection, ForceMode.Impulse);
            _crushed = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player")) return;
            if (_crushed)
            {
                if (!_stopped)
                {
                    rb.isKinematic = true;
                    _stopped = true;
                    _stoppedPosition = transform.position;
                    if (infinitePushes) _crushed = false;
                }
            }
        }
        
    }
}