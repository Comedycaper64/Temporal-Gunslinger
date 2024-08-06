using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;

public class FocusVolumeManager : MonoBehaviour
{
    private Volume focusVolume;
    private bool bFocusChanged = false;
    private float tweenTimer = 0f;
    private float tweenSpeed = 0.4f;
    private float targetVolume = 0f;
    private float nonTargetVolume = 1f;

    private void Awake()
    {
        focusVolume = GetComponent<Volume>();
        FocusManager.OnFocusToggle += ToggleFocusVolume;
    }

    private void Update()
    {
        if (bFocusChanged)
        {
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                1f,
                nonTargetVolume,
                targetVolume,
                MMTween.MMTweenCurve.EaseOutExponential
            );

            tweenTimer += tweenSpeed * Time.unscaledDeltaTime;

            focusVolume.weight = lerp;

            if (Mathf.Abs(focusVolume.weight - targetVolume) < 0.01f)
            {
                tweenTimer = 0f;
                bFocusChanged = false;
            }
        }
    }

    private void ToggleFocusVolume(object sender, bool toggle)
    {
        // if (!GameManager.bLevelActive)
        // {
        //     return;
        // }

        if (toggle)
        {
            targetVolume = 1f;
            nonTargetVolume = 0f;
        }
        else
        {
            targetVolume = 0f;
            nonTargetVolume = 1f;
        }

        if (bFocusChanged)
        {
            tweenTimer = 1 - tweenTimer;
        }
        else
        {
            tweenTimer = 0f;
        }

        bFocusChanged = true;
    }
}
