using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 1f;
    private bool bFade;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;
    private float tweenTimer;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Update()
    {
        if (bFade)
        {
            float newAlpha = MMTween.Tween(
                tweenTimer,
                0f,
                fadeTime,
                currentAlpha,
                targetAlpha,
                MMTween.MMTweenCurve.EaseInOutExponential
            );
            canvasGroup.alpha = newAlpha;
            tweenTimer += Time.unscaledDeltaTime;

            if (newAlpha == targetAlpha)
            {
                bFade = false;
            }
        }
    }

    private void FadeIn(float targetAlpha)
    {
        this.targetAlpha = targetAlpha;
    }

    private void FadeOut()
    {
        targetAlpha = 0f;
    }

    public void SetCanvasGroupAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
        bFade = false;
    }

    public float GetCanvasGroupAlpha()
    {
        return canvasGroup.alpha;
    }

    public void ToggleFade(bool toggle, float targetAlpha = 1.0f)
    {
        if (toggle)
        {
            FadeIn(targetAlpha);
        }
        else
        {
            FadeOut();
        }

        // if (targetAlpha == canvasGroup.alpha)
        // {
        //     return;
        // }

        bFade = true;
        tweenTimer = 0f;
        currentAlpha = canvasGroup.alpha;
    }
}
