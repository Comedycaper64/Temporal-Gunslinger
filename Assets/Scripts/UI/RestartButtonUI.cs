using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonUI : MonoBehaviour
{
    [SerializeField]
    private Image progressUI;
    private Material progressBar;

    private void Awake()
    {
        progressBar = progressUI.material;
        progressBar.SetFloat("_Progress", 0f);
    }

    private void OnEnable()
    {
        RewindManager.OnRestartTimerChanged += RewindManager_OnRestartTimerChanged;
    }

    private void OnDisable()
    {
        RewindManager.OnRestartTimerChanged -= RewindManager_OnRestartTimerChanged;
    }

    private void RewindManager_OnRestartTimerChanged(object sender, float newValue)
    {
        progressBar.SetFloat("_Progress", newValue);
    }
}
