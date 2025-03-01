using System;
using Cinemachine;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private bool shouldGunMove;
    private float gunMoveTime = 0.1f;

    //private float distanceAllowance = 0.001f;
    private const float lerpSpeed = 3f;
    private Vector3 gunVelocity = Vector3.zero;
    private Transform target;

    [SerializeField]
    private Material opaqueGunMaterial;

    [SerializeField]
    private Material transGunMaterial;

    private const float FOCUS_ALPHA = 0.2f;
    private float alphaNonTarget = FOCUS_ALPHA;
    private float alphaTarget = 1f;
    private const float GUN_REST_FOV = 50f;
    private const float GUN_AIM_FOV = 25f;
    private float fovTarget = GUN_REST_FOV;
    private float fovNonTarget = GUN_AIM_FOV;

    [SerializeField]
    private Transform gunModel;

    [SerializeField]
    private Renderer[] gunModelRenderers;

    [SerializeField]
    private Transform standbyPosition;

    [SerializeField]
    private Transform aimingPosition;

    [SerializeField]
    private Transform bulletPosition;

    [SerializeField]
    private AudioClip aimGunSFX;

    [SerializeField]
    private AudioClip shootSFX;

    [SerializeField]
    private VFXPlayback gunShotVFX;

    [SerializeField]
    private BulletStateMachine bulletStateMachine;

    [SerializeField]
    private CinemachineVirtualCamera gunPOVCamera;
    private Bullet bullet;
    private BulletPossessTarget bulletPossessTarget;
    private CinemachineImpulseSource impulseSource;
    public static EventHandler<bool> OnAimGun;

    private void Start()
    {
        bullet = bulletStateMachine.GetComponent<Bullet>();
        bulletPossessTarget = bulletStateMachine.GetComponent<BulletPossessTarget>();

        impulseSource = GetComponent<CinemachineImpulseSource>();
        foreach (Renderer renderer in gunModelRenderers)
        {
            renderer.material = opaqueGunMaterial;
        }
    }

    private void Update()
    {
        if (!shouldGunMove)
        {
            return;
        }

        gunModel.position = Vector3.SmoothDamp(
            gunModel.position,
            target.position,
            ref gunVelocity,
            gunMoveTime
        );

        float lerpRatio = Mathf.Abs(
            Mathf.InverseLerp(fovNonTarget, fovTarget, gunPOVCamera.m_Lens.FieldOfView)
        );

        float newLerp = lerpRatio + (lerpSpeed * Time.unscaledDeltaTime);

        float newAlpha = Mathf.Lerp(alphaNonTarget, alphaTarget, newLerp);

        float newFOV = Mathf.Lerp(fovNonTarget, fovTarget, newLerp);

        foreach (Renderer renderer in gunModelRenderers)
        {
            Material material = renderer.material;
            material.color = new Color(
                material.color.r,
                material.color.g,
                material.color.b,
                newAlpha
            );
        }

        gunPOVCamera.m_Lens.FieldOfView = newFOV;

        if (Math.Abs(gunModelRenderers[0].material.color.a - alphaTarget) < 0.01f)
        {
            shouldGunMove = false;
        }
        // if (Vector3.Distance(gunModel.position, target.position) < distanceAllowance)
        // {
        //     shouldGunMove = false;
        // }
    }

    public void SetGunStandbyPosition()
    {
        gunModel.position = standbyPosition.position;

        gunPOVCamera.m_Lens.FieldOfView = GUN_REST_FOV;

        foreach (Renderer renderer in gunModelRenderers)
        {
            Material material = renderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
        }
    }

    public void ToggleAimGun(bool toggle)
    {
        if (toggle)
        {
            target = aimingPosition;
            alphaTarget = FOCUS_ALPHA;
            alphaNonTarget = 1f;

            fovTarget = GUN_AIM_FOV;
            fovNonTarget = GUN_REST_FOV;

            AudioManager.PlaySFX(aimGunSFX, 0.3f, 3, transform.position);

            foreach (Renderer renderer in gunModelRenderers)
            {
                renderer.material = transGunMaterial;
            }
        }
        else
        {
            target = standbyPosition;
            alphaTarget = 1f;
            alphaNonTarget = FOCUS_ALPHA;

            fovTarget = GUN_REST_FOV;
            fovNonTarget = GUN_AIM_FOV;

            foreach (Renderer renderer in gunModelRenderers)
            {
                renderer.material = opaqueGunMaterial;
            }
        }

        bulletPossessTarget.ToggleNearbyPossessableHighlight(toggle);

        OnAimGun?.Invoke(this, toggle);

        shouldGunMove = true;
    }

    public void FireGun()
    {
        AudioManager.PlaySFX(shootSFX, 0.6f, 0, transform.position);
        gunShotVFX.PlayEffect();
        impulseSource.GenerateImpulse();
        bulletPossessTarget.ToggleNearbyPossessableHighlight(false);
        bulletStateMachine.SwitchToActive();
    }

    public void ResetBullet()
    {
        bullet.ResetBullet(bulletPosition);
    }

    // public void DisableBullet()
    // {
    //     bullet.SwitchToInactive();
    // }
}
