using UnityEngine;
using UnityEngine.Events;

namespace EnvironmentObjects.Triggers
{
    public class TeleportTrigger : PlayerTrigger
    {
        [SerializeField] private Transform teleportPoint;

        protected override void OnPlayerEnter(Collider player)
        {
            Player.Player.Instance.PlayerData.ControlledCollider.SetPosition(teleportPoint.position);
            Player.Player.Instance.PlayerData.Spear.transform.position = teleportPoint.position;
        }
    }
}