using GlobalManagers;
using Levels;
using UnityEngine;
using Utils;

namespace EnvironmentObjects.Triggers
{
    public class SceneLoadTrigger : PlayerTrigger
    {
        [SerializeField] private SceneField[] scenesToLoad;
        [SerializeField] private SceneField[] scenesToUnload;
        protected override void OnPlayerEnter(Collider player1)
        {
            LevelManager.Instance.LoadScenes(scenesToLoad);
            LevelManager.Instance.UnloadScenes(scenesToUnload);
        }
    }
}