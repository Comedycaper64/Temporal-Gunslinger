using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedirectUI : MonoBehaviour
{
    // private bool uiToggle = false;
    // private float currentAlpha = 0f;
    // private float targetAlpha = 0f;
    // private float tweenTimer = 0f;
    [SerializeField]
    private Color noCoinsColour;

    [SerializeField]
    private Color coinChangeColour;

    [SerializeField]
    private Gradient noCoinsGradient;

    [SerializeField]
    private Gradient coinChangeGradient;

    private MMF_Player flashPlayer;
    private CanvasGroupFader canvasFader;

    // [SerializeField]
    // private CanvasGroup redirectUI;

    [SerializeField]
    private TextMeshProUGUI redirectText;

    private void Awake()
    {
        RedirectManager.OnRedirectsChanged += UpdateText;
        RedirectManager.OnRedirectUIActive += ToggleUI;
        RedirectManager.OnRedirectFailed += FlashNoCoins;

        flashPlayer = GetComponent<MMF_Player>();
        canvasFader = GetComponent<CanvasGroupFader>();
        canvasFader.SetCanvasGroupAlpha(0f);
    }

    // private void Update()
    // {
    //     if (uiToggle)
    //     {
    //         float newAlpha = MMTween.Tween(
    //             tweenTimer,
    //             0f,
    //             1f,
    //             currentAlpha,
    //             targetAlpha,
    //             MMTween.MMTweenCurve.EaseInOutExponential
    //         );
    //         redirectUI.alpha = newAlpha;
    //         tweenTimer += Time.deltaTime;

    //         if (newAlpha == targetAlpha)
    //         {
    //             uiToggle = false;
    //         }
    //     }
    // }

    private void FlashNoCoins()
    {
        flashPlayer.GetFeedbackOfType<MMF_TMPColor>().DestinationColor = noCoinsColour;
        flashPlayer.GetFeedbackOfType<MMF_Image>().ColorOverTime = noCoinsGradient;
        PlayFeedback();
    }

    private void ToggleUI(object sender, bool e)
    {
        canvasFader.ToggleFade(e);
        // uiToggle = true;
        // tweenTimer = 0f;
        // currentAlpha = redirectUI.alpha;

        // if (e)
        // {
        //     targetAlpha = 1f;
        // }
        // else
        // {
        //     targetAlpha = 0f;
        // }
    }

    private void UpdateText(object sender, int e)
    {
        redirectText.text = "x " + e.ToString();
        flashPlayer.GetFeedbackOfType<MMF_TMPColor>().DestinationColor = coinChangeColour;
        flashPlayer.GetFeedbackOfType<MMF_Image>().ColorOverTime = coinChangeGradient;
        PlayFeedback();
    }

    private void PlayFeedback()
    {
        flashPlayer.PlayFeedbacks();
    }

    private void OnDisable()
    {
        RedirectManager.OnRedirectsChanged -= UpdateText;
        RedirectManager.OnRedirectUIActive -= ToggleUI;
        RedirectManager.OnRedirectFailed -= FlashNoCoins;
    }
}
