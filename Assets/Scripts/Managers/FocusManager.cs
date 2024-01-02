using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;
    private float focusTimeScale = 0.5f;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
        PlayerStateMachine.OnPlayerStateChanged += ToggleCanFocus;
    }

    private void OnDisable()
    {
        PlayerStateMachine.OnPlayerStateChanged -= ToggleCanFocus;
    }

    private void Update()
    {
        if (!bCanFocus)
        {
            return;
        }

        if (inputManager.GetIsFocusing() != bFocusing)
        {
            ToggleFocusing(inputManager.GetIsFocusing());
        }
    }

    private void ToggleCanFocus(object sender, State e)
    {
        if (e.GetType() == typeof(PlayerBulletState))
        {
            bCanFocus = true;
        }
        else
        {
            bCanFocus = false;
            ToggleFocusing(false);
        }
    }

    private void ToggleFocusing(bool isFocusing)
    {
        bFocusing = isFocusing;

        if (bFocusing)
        {
            Focus();
        }
        else
        {
            UnFocus();
        }
    }

    private void UnFocus()
    {
        Time.timeScale = 1f;
    }

    private void Focus()
    {
        Time.timeScale = focusTimeScale;
    }
}
