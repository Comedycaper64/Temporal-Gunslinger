using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshairUI;

    private void Start()
    {
        FocusManager.OnFocusToggle += ToggleUI;
    }

    private void OnDisable()
    {
        FocusManager.OnFocusToggle -= ToggleUI;
    }

    private void ToggleUI(object sender, bool toggle)
    {
        crosshairUI.SetActive(toggle);
    }
}