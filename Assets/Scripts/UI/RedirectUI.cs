using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class RedirectUI : MonoBehaviour
{
    private bool uiToggle = false;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;
    private float tweenTimer = 0f;

    private MMF_Player flashPlayer;

    [SerializeField]
    private CanvasGroup redirectUI;

    [SerializeField]
    private TextMeshProUGUI redirectText;

    private void Awake()
    {
        RedirectManager.OnRedirectsChanged += UpdateText;
        RedirectManager.OnRedirectUIActive += ToggleUI;
        RedirectManager.OnRedirectFailed += FlashUI;

        flashPlayer = GetComponent<MMF_Player>();
        redirectUI.alpha = 0f;
    }

    private void Update()
    {
        if (uiToggle)
        {
            float newAlpha = MMTween.Tween(
                tweenTimer,
                0f,
                1f,
                currentAlpha,
                targetAlpha,
                MMTween.MMTweenCurve.EaseInOutExponential
            );
            redirectUI.alpha = newAlpha;
            tweenTimer += Time.deltaTime;

            if (newAlpha == targetAlpha)
            {
                uiToggle = false;
            }
        }
    }

    private void FlashUI()
    {
        flashPlayer.PlayFeedbacks();
    }

    private void ToggleUI(object sender, bool e)
    {
        uiToggle = true;
        tweenTimer = 0f;
        currentAlpha = redirectUI.alpha;

        if (e)
        {
            targetAlpha = 1f;
        }
        else
        {
            targetAlpha = 0f;
        }
    }

    private void UpdateText(object sender, int e)
    {
        redirectText.text = "x " + e.ToString();
    }

    private void OnDisable()
    {
        RedirectManager.OnRedirectsChanged -= UpdateText;
        RedirectManager.OnRedirectUIActive -= ToggleUI;
        RedirectManager.OnRedirectFailed -= FlashUI;
    }
}
