using System;
using CommonObjects;
using GlobalManagers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

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
        [SerializeField] private bool inverse;
        [SerializeField] private DirectionAllowed directionAllowed;
        [SerializeField] private bool fixedVelocity;
        [ShowIf("fixedVelocity")] 
        [SerializeField] private float constVelocity;
        [SerializeField] private float minVelocity;

        [Header("Arrow Visuals")] 
        [SerializeField] private Color inverseColor;
        [SerializeField] private Color normalColor;
        [SerializeField] private SpriteRenderer mixedArrow;
        [SerializeField] private SpriteRenderer anyDirectionArrow;
        [SerializeField] private SpriteRenderer directionArrow;
        [SerializeField] private SpriteRenderer doubleDirectionArrow;
        [SerializeField] private SpriteRenderer infiniteArrow;
        
        private bool _crushed;
        private bool _stopped;
        
        private float _restoreTimer;
        private Vector3 _startPosition;
        private Vector3 _stoppedPosition;

        private Vector2 _velocityCrushedWith;
        
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

            InitArrows();

        }

        private void InitArrows()
        {
            mixedArrow.gameObject.SetActive(false);
            doubleDirectionArrow.gameObject.SetActive(false);
            anyDirectionArrow.gameObject.SetActive(false);
            directionArrow.gameObject.SetActive(false);
            
            infiniteArrow.gameObject.SetActive(restorable);
            SpriteRenderer arrow;
            switch (directionAllowed)
            {
                case DirectionAllowed.Any:
                    arrow = anyDirectionArrow;
                    break;
                case DirectionAllowed.Right or DirectionAllowed.Up:
                    arrow = inverse ? mixedArrow : doubleDirectionArrow;
                    break;
                default:
                    arrow = directionArrow;
                    break;
            }

            switch (directionAllowed)
            {
                case DirectionAllowed.Right:
                    arrow.transform.right = transform.right;
                    break;
                case DirectionAllowed.Up:
                    arrow.transform.up = transform.up;
                    break;
            }
            
            arrow.gameObject.SetActive(true);
            arrow.color = inverse ? inverseColor : normalColor;

        }

        private void Update()
        {
            if (restorable && _stopped)
            { 
                _restoreTimer += Time.deltaTime;
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
            if (restorable && _stopped)
            {
                transform.position = Vector3.Lerp(_stoppedPosition, _startPosition,
                    positionLerp.Evaluate(_restoreTimer / restoreTime));
            }
            else
            {
                if (_crushed)
                {
                    switch (directionAllowed)
                    {
                        case DirectionAllowed.Any:
                            break;
                        case DirectionAllowed.Right:
                            rb.linearVelocity = Vector3.Project(rb.linearVelocity, transform.right);
                            break;
                        case DirectionAllowed.Up:
                            rb.linearVelocity = Vector3.Project(rb.linearVelocity, transform.up);
                            break;
                    }

                }
            }
        }
        
        public void Crush(Vector2 crushDirection)
        {
            if (_crushed)
            {
                if (!infinitePushes) return;
            }
            if (inverse) crushDirection = -crushDirection;
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

            if (crushDirection == Vector2.zero)
            {
                
                switch (directionAllowed)
                {
                    case DirectionAllowed.Any:
                        break;
                    case DirectionAllowed.Right:
                        crushDirection = transform.right;
                        break;
                    case DirectionAllowed.Up:
                        crushDirection = Vector3.Project(crushDirection, transform.up);
                        break;
                }
            }
            
            rb.isKinematic = false;
            _stopped = false;
            _velocityCrushedWith = crushDirection;
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
                }
            }
        }
        
    }
}