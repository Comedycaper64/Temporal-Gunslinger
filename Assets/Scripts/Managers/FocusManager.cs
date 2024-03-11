using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;
    private const float NORMAL_FOV = 40f;
    private const float FOCUS_FOV = 25f;
    private float focusZoomSpeed = 10f;
    private float targetFOV = 40f;
    private float nonTargetFOV = 25f;

    [SerializeField]
    private Transform bulletModelTransform;
    private AimLine focusAimLine;

    [SerializeField]
    private CinemachineFreeLook bulletCamera;

    //private LensSettings cameraLensSettings;
    public static EventHandler<bool> OnFocusToggle;

    private void Start()
    {
        CreateAimLine();
    }

    private void Update()
    {
        focusAimLine.UpdateLineDirection(bulletModelTransform.forward);

        if (bulletCamera.m_Lens.FieldOfView != targetFOV)
        {
            float lerpRatio =
                1
                - (
                    Mathf.Abs(bulletCamera.m_Lens.FieldOfView - targetFOV)
                    / Mathf.Abs(nonTargetFOV - targetFOV)
                );
            //Debug.Log("A: " + (bulletCamera.m_Lens.FieldOfView - targetFOV));
            //Debug.Log("B: " + (nonTargetFOV - targetFOV));
            //Debug.Log("Ratio: " + lerpRatio);
            bulletCamera.m_Lens.FieldOfView = Mathf.Lerp(
                nonTargetFOV,
                targetFOV,
                lerpRatio + (focusZoomSpeed * Time.deltaTime)
            );

            //Debug.Log("Lens: " + bulletCamera.m_Lens.FieldOfView);
        }

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
        targetFOV = NORMAL_FOV;
        nonTargetFOV = FOCUS_FOV;
        OnFocusToggle?.Invoke(this, false);
    }

    private void Focus()
    {
        TimeManager.SetSlowedTime(true);
        targetFOV = FOCUS_FOV;
        nonTargetFOV = NORMAL_FOV;
        OnFocusToggle?.Invoke(this, true);
    }

    public bool IsFocusing()
    {
        return bFocusing;
    }

    public Vector3 GetAimDirection()
    {
        return Camera.main.transform.forward;
        // if (bFocusing)
        // {
        //     return Camera.main.transform.forward;
        // }
        // else
        // {
        //     return focusAimLine.GetLineDirection();
        // }
    }
}
