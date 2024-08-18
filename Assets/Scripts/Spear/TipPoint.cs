using CommonObjects;
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
        
        public bool IsInHardGround { get; private set; }
        public bool CanBeLocked { get; private set; }
        public bool IsLocked { get; private set; }
        

        public void Init(SpearData spearData)
        {
            _data = spearData;
            _baseParent = transform.parent;
        }

        public bool Lock()
        {
            if (!CanBeLocked) return false;
            if (IsLocked) return true;
            _lockedScale = _data.Scale;
            IsLocked = true;
            transform.SetParent(_lastOtherObject);
            return true;
        }

        public void UnLock()
        {
            if (!IsLocked) return;
            IsLocked = false;
            CanBeLocked = false;
            IsInHardGround = false;
            transform.SetParent(_baseParent);
            var vector3 = transform.localPosition;
            vector3.y = 0f;
            vector3.z = 0f;
            transform.localPosition = vector3;
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
