using System;
using Cinemachine;
using MoreMountains.Tools;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    private bool bCanFocus = false;
    private bool bFocusing = false;
    private const float NORMAL_CAMERA_Y = 3f;
    private const float NORMAL_CAMERA_X = 225f;
    private const float FOCUS_CAMERA_Y = 1.5f;
    private const float FOCUS_CAMERA_X = 120f;
    private float PLAYER_SENSITIVITY = 1f;
    private const float NORMAL_FOV = 50f;
    private const float FOCUS_FOV = 30f;
    private const float focusZoomSpeed = 1.5f;

    // private const float focusAlpha = 0.25f;
    private float tweenTimer = 0f;

    // private float alphaNonTarget = focusAlpha;
    // private float alphaTarget = 1f;
    private float targetFOV = NORMAL_FOV;
    private float nonTargetFOV = FOCUS_FOV;

    [SerializeField]
    private bool bSpawnAimLine = true;

    [SerializeField]
    private float cameraSpecificSensitivityMult = 1f;

    [SerializeField]
    private Collider bulletCollider;

    [SerializeField]
    private Transform bulletModelTransform;

    [SerializeField]
    private AudioClip focusSFX;

    // [SerializeField]
    // private Renderer[] modelRenderer;

    // [SerializeField]
    // private Material roughMaterial;

    // [SerializeField]
    // private Material transparentMaterial;
    [SerializeField]
    private GameObject dissolveModel;

    [SerializeField]
    private GameObject transparentModel;

    private AimLine focusAimLine;

    [SerializeField]
    private CinemachineFreeLook bulletCamera;
    private CinemachineImpulseListener cameraImpulseListener;

    public static EventHandler<bool> OnFocusToggle;

    private void Awake()
    {
        cameraImpulseListener = bulletCamera.GetComponent<CinemachineImpulseListener>();
        PLAYER_SENSITIVITY = PlayerOptions.GetBulletSensitivity();
        // new Vector2(
        //     PlayerOptions.GetBulletSensitivity(),
        //     PlayerOptions.GetBulletYSensitivity()
        // );
        bulletCamera.m_XAxis.m_MaxSpeed =
            NORMAL_CAMERA_X * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
        bulletCamera.m_YAxis.m_MaxSpeed =
            NORMAL_CAMERA_Y * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
    }

    private void Start()
    {
        if (bSpawnAimLine)
        {
            CreateAimLine();
        }
    }

    private void OnEnable()
    {
        OptionsManager.OnBulletSensitivityUpdated += OnSensitivityUpdated;
    }

    private void OnDisable()
    {
        OptionsManager.OnBulletSensitivityUpdated -= OnSensitivityUpdated;
    }

    private void Update()
    {
        if (focusAimLine)
        {
            focusAimLine.UpdateLineDirection(bulletModelTransform.forward);
        }

        if (bulletCamera.m_Lens.FieldOfView != targetFOV)
        {
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                1f,
                nonTargetFOV,
                targetFOV,
                MMTween.MMTweenCurve.EaseOutExponential
            );

            tweenTimer += focusZoomSpeed * Time.unscaledDeltaTime;

            bulletCamera.m_Lens.FieldOfView = lerp;

            //float newAlpha = Mathf.Lerp(alphaNonTarget, alphaTarget, lerp);

            // if (modelRenderer.Length > 0)
            // {
            //     foreach (Renderer renderer in modelRenderer)
            //     {
            //         Material material = renderer.material;
            //         material.color = new Color(
            //             material.color.r,
            //             material.color.g,
            //             material.color.b,
            //             newAlpha
            //         );
            //     }
            // }
        }

        if (!bCanFocus)
        {
            return;
        }
    }

    private void CreateAimLine()
    {
        float colliderRadius;

        if (bulletCollider.GetType() == typeof(SphereCollider))
        {
            SphereCollider collider = bulletCollider as SphereCollider;
            colliderRadius = collider.radius;
        }
        else if (bulletCollider.GetType() == typeof(BoxCollider))
        {
            BoxCollider collider = bulletCollider as BoxCollider;
            colliderRadius = collider.size.x / 2;
        }
        else
        {
            colliderRadius = 0.025f;
        }

        focusAimLine = AimLineManager.Instance.CreateAimLine(
            bulletModelTransform,
            bulletModelTransform.forward,
            colliderRadius
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

        bulletCamera.m_XAxis.m_MaxSpeed =
            NORMAL_CAMERA_X * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
        bulletCamera.m_YAxis.m_MaxSpeed =
            NORMAL_CAMERA_Y * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;

        dissolveModel.SetActive(true);
        transparentModel.SetActive(false);

        // alphaTarget = 1f;
        // alphaNonTarget = focusAlpha;

        // foreach (Renderer renderer in modelRenderer)
        // {
        //     renderer.material = roughMaterial;
        // }

        tweenTimer = 0f;

        cameraImpulseListener.enabled = true;

        OnFocusToggle?.Invoke(this, false);
    }

    private void Focus()
    {
        TimeManager.SetSlowedTime(true);
        targetFOV = FOCUS_FOV;
        nonTargetFOV = NORMAL_FOV;

        bulletCamera.m_XAxis.m_MaxSpeed =
            FOCUS_CAMERA_X * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
        bulletCamera.m_YAxis.m_MaxSpeed =
            FOCUS_CAMERA_Y * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;

        dissolveModel.SetActive(false);
        transparentModel.SetActive(true);

        // alphaTarget = focusAlpha;
        // alphaNonTarget = 1f;

        // foreach (Renderer renderer in modelRenderer)
        // {
        //     renderer.material = transparentMaterial;
        // }

        AudioManager.PlaySFX(focusSFX, 1f, 0, transform.position);

        tweenTimer = 0f;

        cameraImpulseListener.enabled = false;

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

    public void ToggleAimLine(bool toggle)
    {
        if (focusAimLine)
        {
            focusAimLine.ToggleLine(toggle);
        }
    }

    private void OnSensitivityUpdated(object sender, float newSensitivity)
    {
        PLAYER_SENSITIVITY = newSensitivity;

        if (bFocusing)
        {
            bulletCamera.m_XAxis.m_MaxSpeed =
                FOCUS_CAMERA_X * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
            bulletCamera.m_YAxis.m_MaxSpeed =
                FOCUS_CAMERA_Y * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
        }
        else
        {
            bulletCamera.m_XAxis.m_MaxSpeed =
                NORMAL_CAMERA_X * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
            bulletCamera.m_YAxis.m_MaxSpeed =
                NORMAL_CAMERA_Y * PLAYER_SENSITIVITY * cameraSpecificSensitivityMult;
        }
    }
}
