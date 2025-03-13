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
    }

    private void OnDisable()
    {
        CinematicManager.OnFadeToBlackToggle -= ToggleFade;
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
                    FadeFinished();
                }
            }
            else
            {
                canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                if (canvasGroup.alpha <= 0f)
                {
                    FadeFinished();
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
}
