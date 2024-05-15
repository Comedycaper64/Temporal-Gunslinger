using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private bool shouldGunMove;
    private float gunMoveTime = 0.1f;
    private float distanceAllowance = 0.001f;
    private Vector3 gunVelocity = Vector3.zero;
    private Transform target;

    //private List<Material> gunModelMaterials = new List<Material>();

    [SerializeField]
    private Material opaqueGunMaterial;

    [SerializeField]
    private Material transGunMaterial;

    private const float focusAlpha = 0.1f;
    private float alphaNonTarget = focusAlpha;
    private float alphaTarget = 1f;

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
    private VFXPlayback gunShotVFX;

    [SerializeField]
    private BulletStateMachine bulletStateMachine;
    private Bullet bullet;
    public static EventHandler<bool> OnAimGun;

    private void Start()
    {
        bullet = bulletStateMachine.GetComponent<Bullet>();
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

        float lerpRatio = Mathf.InverseLerp(
            alphaNonTarget,
            alphaTarget,
            gunModelRenderers[0].material.color.a
        );

        //Debug.Log("A: " + (bulletCamera.m_Lens.FieldOfView - targetFOV));
        //Debug.Log("B: " + (nonTargetFOV - targetFOV));
        //Debug.Log("Ratio: " + lerpRatio);
        float newLerp = lerpRatio + (10f * Time.unscaledDeltaTime);

        float newAlpha = Mathf.Lerp(alphaNonTarget, alphaTarget, newLerp);

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

        if (Vector3.Distance(gunModel.position, target.position) < distanceAllowance)
        {
            shouldGunMove = false;
        }
    }

    public void SetGunStandbyPosition()
    {
        gunModel.position = standbyPosition.position;

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

            AudioManager.PlaySFX(aimGunSFX, 1f, transform.position);

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
        //SFX
        gunShotVFX.PlayEffect();
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
