using System;
using CommonObjects;
using EnvironmentObjects;
using NUnit.Framework;
using Spear.Data;
using UnityEngine;
using Utils;

namespace Spear
{
    public class TipPoint : GamePlayBehaviour
    {
        private SpearData _data;
        private Transform _baseParent;
        private Transform _lastOtherObject;
        private float _lockedScale;
        private bool _forceLocked;
        private Vector2 _lastPosition;

        public bool IsInHardGround { get; private set; }
        public bool CanBeLocked { get; private set; }
        public bool IsLocked { get; private set; }
        public Vector2 PositionDelta => (Vector2) transform.position - _lastPosition;

        public void Init(SpearData spearData)
        {
            _data = spearData;
            _baseParent = transform.parent;
        }

        public void Lock(bool forceSolidGround = false)
        {
            if (IsLocked) return;
            if (forceSolidGround)
            {
                IsInHardGround = true;
                _forceLocked = true;
            }
            _lockedScale = _data.Scale;
            IsLocked = true;
            transform.SetParent(_lastOtherObject);
        }

        public void UnLock()
        {
            if (!IsLocked) return;
            IsLocked = false;
            _forceLocked = false;
            CanBeLocked = false;
            IsInHardGround = false;
            transform.SetParent(_baseParent);
            var vector3 = transform.localPosition;
            vector3.y = 0f;
            vector3.z = 0f;
            transform.localPosition = vector3;
        }

        private void LateUpdate()
        {
            _lastPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(_data.GetDamage());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _data.SpearConfig.HardGroundMask))
            {
                CanBeLocked = true;
                IsInHardGround = true;
                _lastOtherObject = other.transform; 
                return;
            }
            
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _data.SpearConfig.LockMask))
            {
                _lastOtherObject = other.transform; 
                CanBeLocked = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_forceLocked)
            {
                return;
            }
            
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _data.SpearConfig.HardGroundMask))
            {
                CanBeLocked = false;
                IsInHardGround = false;
                return;
            }
            
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _data.SpearConfig.LockMask))
            {
                CanBeLocked = false;
            }
        }
    }
}
