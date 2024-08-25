using System;
using EnvironmentObjects;
using EnvironmentObjects.Crushable;
using GlobalManagers;
using Music;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class PrincessEndCutscene : MonoBehaviour, ICrushable
{
    [SerializeField] private GameObject flashBang;
    [SerializeField] private MusicSwitchData musicSwitchData;
    [SerializeField] private SceneField scene;
    private void Start()
    {
        flashBang.SetActive(false);
    }

    public void Crush(Vector2 crushDirection)
    {
        flashBang.SetActive(true);
        MusicManager.Instance.SwitchMusic(musicSwitchData);
        SceneManager.LoadSceneAsync(scene);
    }
}
