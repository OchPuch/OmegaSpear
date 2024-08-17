using System;
using GlobalManagers;
using UnityEngine;

public class ManagersBootstrap : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private TimeManager timeManager;

    private void Awake()
    {
        pauseManager.Init();
        timeManager.Init();
    }
}
