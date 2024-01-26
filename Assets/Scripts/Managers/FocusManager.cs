using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;

    //private Vector3 aimLineDirection;

    [SerializeField]
    private Transform bulletModelTransform;
    private Transform mainCameraTransform;
    private Transform currentAimTransform;
    private AimLine focusAimLine;
    private InputManager inputManager;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        inputManager = InputManager.Instance;
        CreateAimLine();
    }

    private void Update()
    {
        focusAimLine.UpdateLineDirection(currentAimTransform.forward);

        if (!bCanFocus)
        {
            return;
        }

        // if (inputManager.GetIsFocusing() != bFocusing)
        // {
        //     ToggleFocusing(inputManager.GetIsFocusing());
        // }
    }

    private void CreateAimLine()
    {
        focusAimLine = AimLineManager.Instance.CreateAimLine(
            bulletModelTransform,
            bulletModelTransform.forward
        );
        //aimLineDirection = bulletModelTransform.forward;
        SetCurrentAimTransform(bulletModelTransform);
        focusAimLine.ToggleLine(true);
    }

    public void ToggleCanFocus(bool toggle)
    {
        bCanFocus = toggle;
        ToggleFocusing(false);
    }

    public void ToggleFocusing(bool isFocusing)
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
        TimeManager.SetSlowedTime(false);

        if (focusAimLine)
        {
            //aimLineDirection = bulletModelTransform.forward;
            SetCurrentAimTransform(bulletModelTransform);
        }
    }

    private void Focus()
    {
        TimeManager.SetSlowedTime(true);

        if (focusAimLine)
        {
            //aimLineDirection = mainCameraTransform.forward;
            SetCurrentAimTransform(mainCameraTransform);
        }
    }

    private void SetCurrentAimTransform(Transform newTransform)
    {
        currentAimTransform = newTransform;
    }

    public bool IsFocusing()
    {
        return bFocusing;
    }

    public Vector3 GetAimDirection() => focusAimLine.GetLineDirection();
}
