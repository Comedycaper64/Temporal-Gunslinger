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
    private const float focusZoomSpeed = 2f;
    private const float focusAlpha = 0.25f;
    private float alphaNonTarget = focusAlpha;
    private float alphaTarget = 1f;
    private float targetFOV = NORMAL_FOV;
    private float nonTargetFOV = FOCUS_FOV;

    [SerializeField]
    private Transform bulletModelTransform;

    [SerializeField]
    private Renderer[] modelRenderer;
    private List<Material> modelMaterial = new List<Material>();
    private AimLine focusAimLine;

    [SerializeField]
    private CinemachineFreeLook bulletCamera;

    //private LensSettings cameraLensSettings;
    public static EventHandler<bool> OnFocusToggle;

    private void Start()
    {
        CreateAimLine();

        if (modelRenderer.Length > 0)
        {
            foreach (Renderer renderer in modelRenderer)
            {
                modelMaterial.Add(renderer.material);
            }
        }

        //modelMaterial.Add()

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
            float newLerp = lerpRatio + (focusZoomSpeed * Time.unscaledDeltaTime);

            bulletCamera.m_Lens.FieldOfView = Mathf.Lerp(nonTargetFOV, targetFOV, newLerp);

            float newAlpha = Mathf.Lerp(alphaNonTarget, alphaTarget, newLerp);

            if (modelMaterial.Count > 0)
            {
                foreach (Material material in modelMaterial)
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        newAlpha
                    );
                }
            }

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

        alphaTarget = 1f;
        alphaNonTarget = focusAlpha;

        OnFocusToggle?.Invoke(this, false);
    }

    private void Focus()
    {
        TimeManager.SetSlowedTime(true);
        targetFOV = FOCUS_FOV;
        nonTargetFOV = NORMAL_FOV;

        bulletCamera.m_XAxis.m_MaxSpeed = FOCUS_CAMERA_X;
        bulletCamera.m_YAxis.m_MaxSpeed = FOCUS_CAMERA_Y;

        alphaTarget = focusAlpha;
        alphaNonTarget = 1f;

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
