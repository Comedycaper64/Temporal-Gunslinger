using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;
    private const float NORMAL_CAMERA_Y = 3f;
    private const float NORMAL_CAMERA_X = 225f;
    private const float FOCUS_CAMERA_Y = 1.5f;
    private const float FOCUS_CAMERA_X = 120f;
    private const float NORMAL_FOV = 50f;
    private const float FOCUS_FOV = 30f;
    private float focusZoomSpeed = 2f;
    private float targetFOV = NORMAL_FOV;
    private float nonTargetFOV = FOCUS_FOV;

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
        bulletCamera.m_XAxis.m_MaxSpeed = NORMAL_CAMERA_X;
        bulletCamera.m_YAxis.m_MaxSpeed = NORMAL_CAMERA_Y;
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
                lerpRatio + (focusZoomSpeed * Time.unscaledDeltaTime)
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

        bulletCamera.m_XAxis.m_MaxSpeed = NORMAL_CAMERA_X;
        bulletCamera.m_YAxis.m_MaxSpeed = NORMAL_CAMERA_Y;

        OnFocusToggle?.Invoke(this, false);
    }

    private void Focus()
    {
        Debug.Log("ayaya");
        TimeManager.SetSlowedTime(true);
        targetFOV = FOCUS_FOV;
        nonTargetFOV = NORMAL_FOV;

        bulletCamera.m_XAxis.m_MaxSpeed = FOCUS_CAMERA_X;
        bulletCamera.m_YAxis.m_MaxSpeed = FOCUS_CAMERA_Y;

        OnFocusToggle?.Invoke(this, true);
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
            return transform.forward;
        }
    }
}
