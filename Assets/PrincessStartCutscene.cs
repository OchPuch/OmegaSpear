using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class PrincessStartCutscene : MonoBehaviour
{
    public UnityEvent cutSceneStarted;
    public UnityEvent cutSceneEnded;
    [SerializeField] private GameObject leftEye;
    [SerializeField] private GameObject rightEye;
    [SerializeField] private SpriteRenderer princess;
    [SerializeField] private Sprite princessOk;
    [SerializeField] private Sprite princessLook;
    [Space(10)]
    [SerializeField] private AudioSource princessSpeak;
    [SerializeField] private AnimationCurve volumeByRange;
    [SerializeField] private AudioClip princessSmallTalk;
    [SerializeField] private AudioClip princessLongTalk;
    [SerializeField] private float extraWaitTime = 2f;
    [Space(10)] [SerializeField] private GameObject startBubble;
    [SerializeField] private GameObject startBubble2;
    [SerializeField] private GameObject[] speakBubbles;

    private Coroutine _waitingRoutine;
    private bool _startedCutScene;
    private bool _eyesEnabled;
    
    private void Start()
    {
        foreach (var speakBubble in speakBubbles)
        {
            speakBubble.SetActive(false);
        }
        
        DisableEyes();
        princessSpeak.volume =
            volumeByRange.Evaluate(Vector2.Distance(transform.position, Player.Player.Instance.transform.position));
        _waitingRoutine = StartCoroutine(WaitingPrincess());
    }

    private void StopWaiting()
    {
        StopCoroutine(_waitingRoutine);
        startBubble.SetActive(false);
        startBubble2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_startedCutScene) return;
        StopWaiting();
        StartCoroutine(CutSceneRoutine());
    }

    private void Update()
    {
        princessSpeak.volume =
            volumeByRange.Evaluate(Vector2.Distance(transform.position, Player.Player.Instance.transform.position));

        if (_eyesEnabled)
        {
            Vector2 tipPointPosition = Player.Player.Instance.PlayerData.Spear.SpearData.TipPoint.transform.position;
            leftEye.transform.right = tipPointPosition  - (Vector2) leftEye.transform.position;
            rightEye.transform.right = tipPointPosition - (Vector2) rightEye.transform.position;
        }
    }

    private IEnumerator WaitingPrincess()
    {
        while (true)
        {
            startBubble2.SetActive(false);
            startBubble.SetActive(true);
            princessSpeak.PlayOneShot(princessSmallTalk);
            yield return new WaitForSecondsRealtime((float)SoundUtils.GetClipLength(princessSmallTalk) + extraWaitTime);
            startBubble2.SetActive(true);
            startBubble.SetActive(false);
            princessSpeak.PlayOneShot(princessLongTalk);
            yield return new WaitForSecondsRealtime((float)SoundUtils.GetClipLength(princessLongTalk) + extraWaitTime);
        }
    }


    private IEnumerator CutSceneRoutine()
    {
        cutSceneStarted.Invoke();
        Player.Player.Instance.PlayerData.ControlledCollider.SetVelocity(Vector2.zero);
        _startedCutScene = true;
        princess.sprite = princessLook;
        EnableEyes();
        yield return new WaitForSecondsRealtime(0.1f);
        princess.sprite = princessOk;
        DisableEyes();
        yield return new WaitForSecondsRealtime(2f);
        princess.sprite = princessLook;
        EnableEyes();
        yield return new WaitForSecondsRealtime(0.05f);
        princess.sprite = princessOk;
        DisableEyes();
        yield return new WaitForSecondsRealtime(0.1f);
        princess.sprite = princessLook;
        EnableEyes();
        yield return new WaitForSecondsRealtime(0.05f);
        princess.sprite = princessOk;
        DisableEyes();
        yield return new WaitForSecondsRealtime(0.05f);
        princess.sprite = princessLook;
        EnableEyes();
        yield return new WaitForSecondsRealtime(2f);

        bool small = true;
        foreach (var speakBubble in speakBubbles)
        {
            speakBubble.SetActive(true);
            if (small)
            {
                princessSpeak.PlayOneShot(princessSmallTalk);
                yield return new WaitForSecondsRealtime((float)SoundUtils.GetClipLength(princessSmallTalk) + extraWaitTime);
            }
            else
            {
                princessSpeak.PlayOneShot(princessLongTalk);
                yield return new WaitForSecondsRealtime((float)SoundUtils.GetClipLength(princessLongTalk) + extraWaitTime);
            }
            speakBubble.SetActive(false);
            small = !small;
        }

        var scale = transform.localScale;
        scale.x = -transform.localScale.x;
        transform.localScale = scale;
        yield return new WaitForSecondsRealtime(1f);
        cutSceneEnded.Invoke();
    }

    private void DisableEyes()
    {
        _eyesEnabled = false;
        leftEye.SetActive(false);
        rightEye.SetActive(false);

    }

    private void EnableEyes()
    {
        _eyesEnabled = true;
        leftEye.SetActive(true);
        rightEye.SetActive(true);
        
    }
}