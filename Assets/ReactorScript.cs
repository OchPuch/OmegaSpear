using System.Collections;
using GlobalManagers;
using Music;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class ReactorScript : MonoBehaviour
{
    [SerializeField] private SceneField sceneToLoad;
    [SerializeField] private MusicSwitchData musicTrigger;
    [SerializeField] private GameObject flashBang;

    private void Start()
    {
        flashBang.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(BoyNextDoor());
        }
    }

    private IEnumerator BoyNextDoor()
    {
        MusicManager.Instance.SwitchMusic(musicTrigger);
        flashBang.SetActive(true);
        SceneManager.LoadScene(sceneToLoad);
        yield break;
    }
}
