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

    private const float focusAlpha = 0.2f;
    private float alphaNonTarget = focusAlpha;
    private float alphaTarget = 1f;
    private const float gunRestFOV = 50f;
    private const float gunAimFOV = 25f;
    private float fovTarget = gunRestFOV;
    private float fovNonTarget = gunAimFOV;

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
    private CinemachineImpulseSource impulseSource;
    public static EventHandler<bool> OnAimGun;

    private void Start()
    {
        bullet = bulletStateMachine.GetComponent<Bullet>();
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

        gunPOVCamera.m_Lens.FieldOfView = gunRestFOV;

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
            alphaTarget = focusAlpha;
            alphaNonTarget = 1f;

            fovTarget = gunAimFOV;
            fovNonTarget = gunRestFOV;

            AudioManager.PlaySFX(aimGunSFX, 0.3f, 5, transform.position);

            foreach (Renderer renderer in gunModelRenderers)
            {
                renderer.material = transGunMaterial;
            }

            OnAimGun?.Invoke(this, true);
        }
        else
        {
            target = standbyPosition;
            alphaTarget = 1f;
            alphaNonTarget = focusAlpha;

            fovTarget = gunRestFOV;
            fovNonTarget = gunAimFOV;

            foreach (Renderer renderer in gunModelRenderers)
            {
                renderer.material = opaqueGunMaterial;
            }

            OnAimGun?.Invoke(this, false);
        }
        shouldGunMove = true;
    }

    public void FireGun()
    {
        AudioManager.PlaySFX(shootSFX, 0.6f, 0, transform.position);
        gunShotVFX.PlayEffect();
        impulseSource.GenerateImpulse();
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
