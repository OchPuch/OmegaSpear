using System;
using GlobalManagers;
using Saving;
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

            if (CheckpointManager.Instance.TryGetLastCheckPoint(out var saveData))
            {
                if (saveData.sceneName == gameObject.scene.name || SceneManager.sceneCount > 2)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            
            Player.Player.Instance.PlayerData.ControlledCollider.SetPosition(transform.position);
            Player.Player.Instance.PlayerData.Spear.transform.position = transform.position;
            Player.Player.Instance.PlayerData.ControlledCollider.SetVelocity(Vector2.zero);
            
            Destroy(gameObject);
            
            
        }
    }
}