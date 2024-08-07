using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InspectUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader inspectControlUI;

    [SerializeField]
    private CanvasGroupFader inspectUI;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI revenantText;

    [SerializeField]
    private Transform cameraObjectPlacement;

    private void Awake()
    {
        InspectManager.OnInspect += ToggleUI;
        InspectManager.OnCanInspect += ToggleControlUI;
    }

    private void OnDisable()
    {
        InspectManager.OnInspect -= ToggleUI;
        InspectManager.OnCanInspect -= ToggleControlUI;
    }

    private void ToggleUI(object sender, InspectTarget target)
    {
        if (target == null)
        {
            inspectUI.ToggleFade(false);
            return;
        }

        inspectUI.ToggleFade(true);
    }

    private void ToggleControlUI(object sender, bool toggle)
    {
        inspectControlUI.ToggleFade(toggle);
    }
}
