using CommonObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Spear
{
    public class SpearScaler : GamePlayBehaviour
    {
        [field: SerializeField] public Transform CenterPoint { get; private set; }
        [field: SerializeField] public Transform HandlePoint { get; private set; }
        [field: SerializeField] public Transform TipPoint { get; private set; }
        [field: SerializeField] public Transform BodyTransform { get; private set; }
        
        private float _maxExpandScale = 5f;
        private float _minExpandScale = 0.02f;
        private float _baseScale = 1f;

        protected override void Start()
        {
            base.Start();
            _baseScale = GetScale();
            SetScale(_baseScale);
        }

        public float GetScale()
        {
            return TipPoint.transform.localPosition.x - HandlePoint.transform.localPosition.x;
        }

        public void ChangeScale(float value, float expandMin, float expandMax)
        {
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
            TipPoint.transform.localPosition = HandlePoint.transform.localPosition +
                                               new Vector3(value, 0, 0);
            _minExpandScale = minExpand;
            _maxExpandScale = maxExpand;
            UpdateBodySize();
        }

        public void SetTipPositionWithScaleUpdate(Vector2 position, bool ignoreRestrictions = false)
        {
            CenterPoint.transform.right = position - (Vector2)CenterPoint.position;
            TipPoint.transform.position = position;
            if (!ignoreRestrictions)
            {
                SetScale(GetScale());
            }
            else
            {
                UpdateBodySize();
            }
        }

        private void UpdateBodySize()
        {
            var currentScale = GetScale();
            BodyTransform.position = HandlePoint.position + HandlePoint.right * currentScale / 2;
            var scale = BodyTransform.localScale;
            scale.x = currentScale;
            BodyTransform.localScale = scale;
        }
    }
}