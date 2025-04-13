using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class CinematicManagerUI : MonoBehaviour
{
    [SerializeField]
    private bool initialFadeState = false;

    private bool fadeStatus = false;
    private bool fadeTarget = false;

    [SerializeField]
    private float fadeSpeed = 2f;

    [SerializeField]
    private MMF_Player loadingScreenPlayer;
    private CanvasGroup canvasGroup;
    private Action onFade;
    public static EventHandler<bool> OnFadeStart;
    public static EventHandler<bool> OnFadeEnd;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (initialFadeState)
        {
            canvasGroup.alpha = 1f;
            fadeStatus = true;
            fadeTarget = true;
        }

        CinematicManager.OnFadeToBlackToggle += ToggleFade;
        PauseMenuUI.OnSkipCutscene += RemoveFadeEvent;
    }

    private void OnDisable()
    {
        CinematicManager.OnFadeToBlackToggle -= ToggleFade;
        PauseMenuUI.OnSkipCutscene -= RemoveFadeEvent;
    }

    private void Update()
    {
        if (fadeStatus != fadeTarget)
        {
            if (fadeTarget)
            {
                canvasGroup.alpha += fadeSpeed * Time.deltaTime;
                if (canvasGroup.alpha >= 1f)
                {
                    OnFadeEnd?.Invoke(this, true);
                    FadeFinished();
                }
            }
            else
            {
                canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                if (canvasGroup.alpha <= 0f)
                {
                    FadeFinished();
                    OnFadeEnd?.Invoke(this, false);
                }
            }
        }
    }

    private void FadeFinished()
    {
        fadeStatus = fadeTarget;
        if (onFade != null)
        {
            onFade();
        }
        onFade = null;
    }

    private void ToggleFade(object sender, UIChangeSO uIChange)
    {
        fadeTarget = uIChange.fadeToBlackToggle;

        OnFadeStart?.Invoke(this, fadeTarget);

        if (uIChange.midLevelFade)
        {
            loadingScreenPlayer.gameObject.SetActive(false);
        }
        else
        {
            loadingScreenPlayer.gameObject.SetActive(true);
            loadingScreenPlayer.PlayFeedbacks();
        }

        if (uIChange.waitUntilFaded)
        {
            //Debug.Log("On Fade Set");
            onFade = uIChange?.onFaded;
        }
        else
        {
            if (uIChange.onFaded != null)
            {
                uIChange?.onFaded();
            }
        }
    }

    private void RemoveFadeEvent()
    {
        onFade = null;
    }
}
