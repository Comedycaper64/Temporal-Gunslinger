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

    private MMTween.MMTweenCurve tweenCurve;

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
                tweenCurve
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

    public void ButtonToggleFade(bool toggle)
    {
        ToggleFade(toggle);
    }

    public void ToggleBlockRaycasts(bool toggle)
    {
        canvasGroup.blocksRaycasts = toggle;
    }

    public void ToggleFade(
        bool toggle,
        float targetAlpha = 1.0f,
        MMTween.MMTweenCurve tweenCurve = MMTween.MMTweenCurve.EaseInOutExponential
    )
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
        this.tweenCurve = tweenCurve;
        bFade = true;
        tweenTimer = 0f;
        currentAlpha = canvasGroup.alpha;
    }
}
