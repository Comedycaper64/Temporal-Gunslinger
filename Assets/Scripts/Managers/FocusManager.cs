using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;
    private float focusTimeScale = 0.5f;
    private Transform bulletTransform;
    private Transform mainCameraTransform;
    private AimLine focusAimLine;
    private InputManager inputManager;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        inputManager = InputManager.Instance;
        bulletTransform = transform;
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

        if (bFocusing)
        {
            focusAimLine.UpdateLineDirection(mainCameraTransform.forward);
        }
    }

    public void ToggleCanFocus(bool toggle)
    {
        bCanFocus = toggle;
        ToggleFocusing(false);
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

        if (focusAimLine)
        {
            focusAimLine.ToggleLine(false);
        }
    }

    private void Focus()
    {
        Time.timeScale = focusTimeScale;
        if (!focusAimLine)
        {
            if (!AimLineManager.Instance || !bulletTransform)
            {
                return;
            }
            focusAimLine = AimLineManager.Instance.CreateAimLine(
                bulletTransform,
                mainCameraTransform.forward
            );
        }
        focusAimLine.ToggleLine(true);
    }
}
