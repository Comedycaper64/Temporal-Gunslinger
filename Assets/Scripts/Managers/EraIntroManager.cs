using System.Collections;
using UnityEngine;

public class EraIntroManager : MonoBehaviour
{
    private bool eventSub = false;
    private int index = 0;
    private float initialDelay = 2f;
    private Coroutine delayCoroutine;

    [SerializeField]
    private AudioSource narrationAudioSource;

    [SerializeField]
    private CanvasGroupFader introUI;

    [SerializeField]
    private CanvasGroupFader[] introTextGroups;

    [SerializeField]
    private CanvasGroupFader[] advanceTextGroups;

    [SerializeField]
    private AudioClip[] introTextNarration;

    [SerializeField]
    private CinematicSO introSO;

    private void Awake()
    {
        delayCoroutine = StartCoroutine(InitialDelay());
    }

    private void Start()
    {
        CinematicManager.Instance.PlayCinematic(introSO, null);
    }

    private void OnDisable()
    {
        if (eventSub)
        {
            InputManager.Instance.OnShootAction -= AdvanceNarration;
        }

        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }
    }

    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(initialDelay);

        AdvanceNarration();
        InputManager.Instance.OnShootAction += AdvanceNarration;
        eventSub = true;
        delayCoroutine = null;
    }

    private void AdvanceNarration()
    {
        if (index >= introTextGroups.Length)
        {
            EndIntro();
            return;
        }

        if (index > 0)
        {
            advanceTextGroups[index - 1].ToggleFade(false);
        }

        introTextGroups[index].ToggleFade(true);
        advanceTextGroups[index].ToggleFade(true);

        if (index < introTextNarration.Length)
        {
            narrationAudioSource.clip = introTextNarration[index];
            narrationAudioSource.Play();
        }

        index++;
    }

    private void EndIntro()
    {
        narrationAudioSource.Stop();
        narrationAudioSource.enabled = false;
        InputManager.Instance.OnShootAction -= AdvanceNarration;
        if (introUI)
        {
            introUI.ToggleFade(false);
        }

        GameManager.Instance.PlayIntroCinematic();
    }
}
