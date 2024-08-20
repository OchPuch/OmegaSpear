using GlobalManagers;
using Music;
using UnityEngine;

namespace EnvironmentObjects.Triggers
{
    public class MusicTrigger : PlayerTrigger
    {
        [SerializeField] private MusicSwitchData musicSwitchData;
        
        protected override void OnPlayerEnter(Collider player1)
        {
            MusicManager.Instance.SwitchMusic(musicSwitchData);
        }
    }
}
