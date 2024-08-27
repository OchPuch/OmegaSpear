using UnityEngine;
using System.Collections;
using GlobalManagers;
using Levels;
using Saving;

//--------------------------------------------------------------------
//When the player enters, respawn them
//--------------------------------------------------------------------
public class DeathTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider a_Collider)
    {
        ControlledCapsuleCollider controlledCapsuleCollider = a_Collider.GetComponent<ControlledCapsuleCollider>();
        if (controlledCapsuleCollider != null)
        {
            //Prevent death state to be used if the collider is no-clipping
            if (controlledCapsuleCollider.AreCollisionsActive())
            { 
                Debug.Log("Death triggered by: " + transform.name);
                if (CheckpointManager.Instance.TryGetLastCheckPoint(out var data))
                {
                    LevelManager.Instance.LoadCheckpoint(data);
                }
                else
                {
                    LevelManager.Instance.LoadNewLevel(LevelManager.Instance.LastLoadedLevel);
                }
            }
        }
    }
}
