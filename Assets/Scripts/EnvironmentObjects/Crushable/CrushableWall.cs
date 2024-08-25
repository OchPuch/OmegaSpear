using System;
using CommonObjects;
using UnityEngine;

namespace EnvironmentObjects.Crushable
{
    public class CrushableWall : GamePlayBehaviour, ICrushable
    { 
        [Header("Components")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform lightObject;
        [Header("Restorable")]
        [SerializeField] private AnimationCurve positionLerp;
        [SerializeField] protected bool restorable;
        [SerializeField] private float restoreSpeedFactor = 1;
        [SerializeField] private bool useRestoreTimeInsteadOfFactor;
        [SerializeField] private float restoreTime;
        [Header("Movement")] 
        [SerializeField] private bool onlyForward;
        [SerializeField] private DirectionAllowed directionAllowed;
        [SerializeField] private float constVelocity;
        [Header("Arrow Visuals")] 
        [SerializeField] private bool hideArrows;
        [SerializeField] private SpriteRenderer directionArrow;
        [SerializeField] private SpriteRenderer doubleDirectionArrow;
        [SerializeField] private SpriteRenderer anyDirectionArrow;
        
        protected Vector3 StartPosition;
        protected Vector3 StoppedPosition;

        private bool _crushed;
        private bool _stopped;
        
        private bool _restoring;
        private float _restoreTimer;
        private float _travelTimer;
        
        protected override void Start()
        {
            base.Start();
            StartPosition = transform.position;
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
            doubleDirectionArrow.gameObject.SetActive(false);
            anyDirectionArrow.gameObject.SetActive(false);
            directionArrow.gameObject.SetActive(false);
            if (hideArrows) return;
            
            SpriteRenderer arrow;
            switch (directionAllowed)
            {
                case DirectionAllowed.Any:
                    arrow = anyDirectionArrow;
                    break;
                default:
                    arrow = doubleDirectionArrow;
                    if (onlyForward) arrow = directionArrow;
                    break;
            }

            switch (directionAllowed)
            {
                case DirectionAllowed.Right:
                    arrow.transform.right = transform.right;
                    break;
                case DirectionAllowed.Up:
                    arrow.transform.right = transform.up;
                    break;
            }
            
            arrow.gameObject.SetActive(true);
        }
        
        private void FixedUpdate()
        {
            if (_crushed && !_stopped)
            {
                _travelTimer += Time.fixedDeltaTime;
            }
            
            if (_restoring)
            {
                RestoringFixedUpdate();
                if (useRestoreTimeInsteadOfFactor)
                {
                    _restoreTimer += Time.fixedDeltaTime;
                    if (_restoreTimer >= restoreTime)
                    {
                        CompleteRestore();
                    }
                }
                else
                {
                    _restoreTimer += Time.fixedDeltaTime * restoreSpeedFactor;
                    if (_restoreTimer >= _travelTimer)
                    {
                        CompleteRestore();
                    }
                }
                

            }
        }

        protected virtual void RestoringFixedUpdate()
        {
            float value = useRestoreTimeInsteadOfFactor ? _restoreTimer / restoreTime : _restoreTimer / _travelTimer;
            rb.MovePosition(Vector3.Lerp(StoppedPosition, StartPosition, positionLerp.Evaluate(value)));
            rb.PublishTransform();
        }
        
        public void Crush(Vector2 crushDirection)
        {
            if (_crushed) return;
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
            var crushVelocity = crushDirection.normalized * constVelocity;
            rb.isKinematic = false;
            rb.linearVelocity = crushVelocity;
            _crushed = true;
            OnCrush();
        }

        protected virtual void Restore()
        { 
            _restoring = true;
        }

        protected virtual void CompleteRestore()
        {
            _restoring = false;
            _crushed = false;
            _stopped = false;
            rb.isKinematic = true;
            _travelTimer = 0f;
            _restoreTimer = 0f;
            transform.position = StartPosition;
            rb.linearVelocity = Vector3.zero;
        }

        protected virtual void OnCrush()
        {
            
        }

        protected virtual void OnStop()
        {
            
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (!_crushed) return;
            if (_restoring) return;
            if (other.collider.CompareTag("Player")) return;
            StoppedPosition = transform.position;
            rb.isKinematic = true;
            _stopped = true;
            rb.linearVelocity = Vector3.zero;
            if (restorable) Restore();
            OnStop();
        }
    }
}