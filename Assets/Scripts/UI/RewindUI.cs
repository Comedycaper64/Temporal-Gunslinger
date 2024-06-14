using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindUI : MonoBehaviour
{
    private float clockHandTurnSpeed = 50f;

    //private float fadeSpeed = 5f;

    //private bool rewindUIActive = false;

    //private CanvasGroup rewindCanvasGroup;
    private CanvasGroupFader rewindFader;

    [SerializeField]
    private Transform clockhandTransform;

    private void Start()
    {
        //rewindCanvasGroup = GetComponent<CanvasGroup>();
        rewindFader = GetComponent<CanvasGroupFader>();
        RewindManager.OnRewindToggle += ToggleRewindUI;
    }

    private void OnDisable()
    {
        RewindManager.OnRewindToggle -= ToggleRewindUI;
    }

    private void Update()
    {
        clockhandTransform.eulerAngles += new Vector3(0, 0, clockHandTurnSpeed * Time.deltaTime);

        // if (!rewindUIActive && rewindCanvasGroup.alpha > 0f)
        // {
        //     rewindCanvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
        // }
        // else if (rewindUIActive && rewindCanvasGroup.alpha < 1f)
        // {
        //     rewindCanvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
        // }
    }

    private void ToggleRewindUI(object sender, bool e)
    {
        //rewindUIActive = e;
        rewindFader.ToggleFade(e);
    }
}
