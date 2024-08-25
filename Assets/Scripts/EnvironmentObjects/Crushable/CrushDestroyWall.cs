using GlobalManagers;
using Spear;
using UnityEngine;

namespace EnvironmentObjects.Crushable
{
    public class CrushDestroyWall : CrushableWall
    {
        protected override void RestoringFixedUpdate() { }

        protected override void OnStop()
        {
            base.OnStop();
            EnvironmentObjectsManager.Instance.WallCrushParticleFactory.CreateParticleSystem(transform.position);
            TempHide();
        }
        
        private void TempHide()
        {
            gameObject.SetActive(false);
            transform.position = StartPosition;
            var vector3 = transform.position;
            vector3.z = 50;
            transform.position = vector3;

            var tipPoint = (GetComponentInChildren<TipPoint>());
            if (tipPoint is not null)
            {
                tipPoint.UnLock();
            }
            
            if (restorable) gameObject.SetActive(true);
        }
    }
}