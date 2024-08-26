using System;
using CommonObjects;
using Spear.Data;
using UnityEngine;

namespace Spear
{
    public class SpearScaler : GamePlayBehaviour
    {
        [field: SerializeField] public Transform CenterPoint { get; private set; }
        [field: SerializeField] public Transform HandlePoint { get; private set; }
        [field: SerializeField] public Transform TipPoint { get; private set; }
        [field: SerializeField] public Transform BodyTransform { get; private set; }

        private SpearData _data;

        private float _maxExpandScale = 5f;
        private float _minExpandScale = 0.02f;
        private float _baseScale = 1f;
        private float _baseScaleY;
        private Vector2 _baseHandleLocalPosition;

        public void Init(SpearData spearData)
        {
            _data = spearData;
            _baseHandleLocalPosition = HandlePoint.localPosition;
            _baseScale = GetScale();
            _baseScaleY = BodyTransform.localScale.y;
            SetScale(_baseScale);
        }

        private void LateUpdate()
        {
            if (_data.TipPoint.IsLocked)
            {
                CenterPoint.transform.right = (Vector2)(TipPoint.transform.position - HandlePoint.transform.position);
                UpdateBodySize();
            }
            else
            {
                HandlePoint.localPosition = _baseHandleLocalPosition;
            }
        }

        public float GetScale()
        {
            return Vector2.Distance(HandlePoint.position, TipPoint.position);
        }

        public void ChangeScale(float value, float expandMin, float expandMax)
        {
            if (value == 0)
                return;
            
            float currentScale = GetScale();
            
            if (value < 0)
            {
                if (currentScale + value > expandMax)
                    expandMax = currentScale + value;
                else if (currentScale + value <= expandMin)
                    value = 0;
            }

            if (value > 0)
            {
                if (_data.TipPoint.IsLocked)
                {
                    return;
                }

                // if (Physics.Raycast(_data.SpearScaler.HandlePoint.position, _data.SpearScaler.CenterPoint.right, out var hit,
                //         _data.SpearConfig.NormalSettings.MaxExpand, _data.SpearConfig.HitMask))
                // {
                //     float maximumScale = Vector2.Distance(HandlePoint.position, hit.point);
                //     if (maximumScale < currentScale + value)
                //     {
                //         if (currentScale < maximumScale)
                //         {
                //             return;
                //         }
                //     }
                // }

                if (currentScale + value < expandMin)
                    expandMin = currentScale + value;
                else if (currentScale + value >= expandMax)
                    value = 0;
            }

            SetScale(currentScale + value, expandMin, expandMax);
        }

        public void ResetScale()
        {
            SetScale(_baseScale);
        }

        public void SetScale(float value)
        {
            value = Mathf.Clamp(value, _minExpandScale, _maxExpandScale);
            TipPoint.transform.localPosition = HandlePoint.transform.localPosition +
                                               new Vector3(value, 0, 0);
            UpdateBodySize();
        }

        public void SetScale(float value, float minExpand, float maxExpand)
        {
            value = Mathf.Clamp(value, minExpand, maxExpand);
            if (!_data.TipPoint.IsLocked)
            {
                TipPoint.transform.position = HandlePoint.transform.position + HandlePoint.transform.right * value;
                _minExpandScale = minExpand;
                _maxExpandScale = maxExpand;
                UpdateBodySize();
            }
            else
            {
                HandlePoint.transform.position = TipPoint.transform.position - TipPoint.transform.right * value;
                _minExpandScale = minExpand;
                _maxExpandScale = maxExpand;
                UpdateBodySize();
            }
        }

        public void UpdateFat(float scale)
        {
            var vector3 = BodyTransform.localScale;
            vector3.y = scale * _baseScaleY;
            BodyTransform.localScale = vector3;
        }

        public void UpdateBodySize()
        {
            var currentScale = GetScale();
            BodyTransform.position = HandlePoint.position + HandlePoint.right * currentScale / 2;
            var scale = BodyTransform.localScale;
            scale.x = currentScale;
            BodyTransform.localScale = scale;
        }
    }
}