using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManagerUI : MonoBehaviour
{
    [SerializeField]
    private bool initialFadeState = false;

    private bool fadeStatus = false;
    private bool fadeTarget = false;
    private float fadeSpeed = 2f;
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
        if (uIChange.waitUntilFaded)
        {
            onFade = uIChange.onFaded;
        }
        else
        {
            uIChange.onFaded();
        }
    }
}