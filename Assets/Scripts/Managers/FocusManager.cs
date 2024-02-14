using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;

    [SerializeField]
    private Transform bulletModelTransform;
    private AimLine focusAimLine;

    private void Start()
    {
        CreateAimLine();
    }

    private void Update()
    {
        focusAimLine.UpdateLineDirection(bulletModelTransform.forward);

        if (!bCanFocus)
        {
            return;
        }
    }

    private void CreateAimLine()
    {
        focusAimLine = AimLineManager.Instance.CreateAimLine(
            bulletModelTransform,
            bulletModelTransform.forward
        );
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
    }

    private void Focus()
    {
        TimeManager.SetSlowedTime(true);
    }

    public bool IsFocusing()
    {
        return bFocusing;
    }

    public Vector3 GetAimDirection()
    {
        if (bFocusing)
        {
            return Camera.main.transform.forward;
        }
        else
        {
            return focusAimLine.GetLineDirection();
        }
    }
}
