using System.Collections.Generic;
using UnityEngine;

namespace Levels.UI
{
    public class LevelsMenu : MonoBehaviour
    {
        [SerializeField] private LevelData tutorial;
        [SerializeField] private List<LevelData> levels;
        [Space(10)] 
        [SerializeField] private LevelPresenter levelPresenterPrefab;
        [SerializeField] private Transform contentParent;

        private void Start()
        {
            tutorial.Unlock();
            InitLevels();
        }

        private void InitLevels()
        {
            foreach (var level in levels)
            {
                var presenter = Instantiate(levelPresenterPrefab, contentParent);
                presenter.Init(level);
            }
        }
    }
}