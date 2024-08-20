using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnvironmentObjects
{
    public class Teleporter : MonoBehaviour
    {

        private void FixedUpdate()
        {
            if (Player.Player.Instance is null)
            {
                return;
            }

            if (SceneManager.sceneCount > 2)
            {
                Destroy(gameObject);
                return;
            }
            
            Player.Player.Instance.PlayerData.ControlledCollider.SetPosition(transform.position);
            Player.Player.Instance.PlayerData.Spear.transform.position = transform.position;
            Player.Player.Instance.PlayerData.ControlledCollider.SetVelocity(Vector2.zero);
            
            Destroy(gameObject);
            
            
        }
    }
}