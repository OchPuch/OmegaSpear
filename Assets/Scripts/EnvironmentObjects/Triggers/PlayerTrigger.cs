using CommonObjects;
using UnityEngine;
using UnityEngine.Events;

namespace EnvironmentObjects.Triggers
{
    public class PlayerTrigger : GamePlayBehaviour
    {
        [SerializeField] private UnityEvent playerEntered;
        [SerializeField] private UnityEvent playerExited;
        [SerializeField] protected bool destroyOnEnter = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnPlayerEnter(other);
            playerEntered.Invoke();
            if (destroyOnEnter) Destroy(gameObject);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnPlayerExit(other);
            playerExited.Invoke();
        }
        
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnPlayerStay(other);
        }
        
        
        protected virtual void OnPlayerEnter(Collider player){}
        protected virtual void OnPlayerStay(Collider player) { }
        protected virtual void OnPlayerExit(Collider player) { }

       
    }
    
}