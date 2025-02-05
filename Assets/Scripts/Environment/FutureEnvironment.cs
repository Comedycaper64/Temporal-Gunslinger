using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class FutureEnvironment : RewindableMovement
{
    private bool reveal = false;
    private bool unreveal = false;

    private float tweenTimer = 0f;
    private float revealRadius = 5f;
    private float timeToReveal = 0.1f;
    private float timeToUnreveal = 0.5f;

    private void Update()
    {
        if (reveal)
        {
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                timeToReveal,
                0f,
                revealRadius,
                MMTween.MMTweenCurve.EaseOutExponential
            );

            Shader.SetGlobalFloat("_RevealMaskRadius", lerp);

            tweenTimer += GetSpeed() * Time.deltaTime;

            if (tweenTimer >= timeToReveal)
            {
                reveal = false;
                unreveal = true;
                tweenTimer = 0f;
            }
        }
        else if (unreveal)
        {
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                timeToUnreveal,
                revealRadius,
                0f,
                MMTween.MMTweenCurve.EaseInExponential
            );

            Shader.SetGlobalFloat("_RevealMaskRadius", lerp);

            tweenTimer += GetSpeed() * Time.deltaTime;

            if (tweenTimer >= timeToReveal)
            {
                unreveal = false;
                tweenTimer = 0f;
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ImpactEffectFuture.OnHitEnvironment += SetNewRevealPosition;
        ToggleMovement(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ImpactEffectFuture.OnHitEnvironment -= SetNewRevealPosition;
    }

    private void SetNewRevealPosition(object sender, Vector3 revealPos)
    {
        reveal = true;
        unreveal = false;
        tweenTimer = 0f;

        Shader.SetGlobalFloat("_RevealMaskRadius", 0f);
        Shader.SetGlobalVector("_RevealMaskPosition", revealPos);
    }
}
