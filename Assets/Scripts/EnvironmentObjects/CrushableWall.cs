using System;
using System.Collections;
using CommonObjects;
using GlobalManagers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnvironmentObjects
{
    public class CrushableWall : GamePlayBehaviour, ICrushable
    {
        [SerializeField] private bool restorable;
        [SerializeField] private Transform lightObject;
        [SerializeField] private float restoreTime;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private DirectionAllowed directionAllowed;
        [SerializeField] private bool fixedVelocity;
        [Min(51)]  [SerializeField] private float maxDistanceFromStart;

        [ShowIf("fixedVelocity")] [SerializeField]
        private float constVelocity;

        private float _restoreTimer;
        private bool _crushed;
        private Vector3 _startPosition;

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
            if (Vector2.Distance(transform.position, _startPosition) > maxDistanceFromStart)
            {
                if (restorable)
                {
                    TempHide();
                }
                else
                {
                    Kill();
                }
            }
        }


        private IEnumerator WaitBeforeRestortion()
        {
            _restoreTimer = 0f;
            while (true)
            {
                if (PauseManager.Instance.IsPaused)
                {
                    yield return null;
                    continue;
                }


                _restoreTimer += Time.deltaTime;
                if (_restoreTimer > restoreTime)
                {
                    rb.isKinematic = true;
                    _crushed = false;
                    transform.position = _startPosition;
                    yield break;
                }

                yield return null;
            }
        }


        public void Crush(Vector2 crushDirection)
        {
            if (_crushed) return;
            rb.isKinematic = false;
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
                EnvironmentObjectsManager.Instance.WallCrushParticleFactory.CreateParticleSystem(transform.position);
                if (!restorable)
                {
                    Kill();
                }
                else
                {
                    TempHide();
                }
            }
        }

        private void Kill()
        {
            transform.DetachChildren();
            Destroy(gameObject);

            
        }
        

        private void TempHide()
        { 
            StopAllCoroutines();
            _crushed = false;
            rb.isKinematic = true;
            gameObject.SetActive(false);
            transform.position = _startPosition;
            var vector3 = transform.position;
            vector3.z = 50;
            transform.position = vector3;
            gameObject.SetActive(true);
            StartCoroutine(WaitBeforeRestortion());
        }
    }
}