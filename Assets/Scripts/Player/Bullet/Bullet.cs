using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private bool bBulletActive;
    private bool bBulletPossessed;
    private Transform gunParent;
    private BulletMovement bulletMovement;
    private BulletCameraController bulletCameraController;
    private BulletStateMachine bulletStateMachine;

    private FocusManager focusManager;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletCameraController = GetComponent<BulletCameraController>();
        bulletStateMachine = GetComponent<BulletStateMachine>();
        focusManager = GetComponent<FocusManager>();
        gunParent = transform.parent;
    }

    private void Update()
    {
        if (bBulletActive)
        {
            bulletMovement.LoseVelocity();

            if (bulletMovement.ShouldBulletDrop())
            {
                bulletStateMachine.SwitchToDeadState();
            }
        }

        if (bBulletPossessed)
        {
            BulletVelocityUI.Instance.VelocityChanged(bulletMovement.GetVelocity());
            //TimeManager.UpdateTimeScale(1f / bulletMovement.GetVelocity());
            RewindableMovement.UpdateMovementTimescale(1f / bulletMovement.GetVelocity());
        }
    }

    public void RedirectBullet()
    {
        if (focusManager.IsFocusing())
        {
            Vector3 aimDirection = focusManager.GetAimDirection();
            bulletMovement.RedirectBullet(aimDirection, GetAimRotation(aimDirection));
        }
        else
        {
            //Sound effect or other indicator
        }
    }

    private Quaternion GetAimRotation(Vector3 aimDirection)
    {
        return Quaternion.LookRotation(aimDirection);
    }

    public void ToggleBulletActive(bool toggle)
    {
        bulletMovement.ToggleMovement(toggle);
        bulletMovement.ToggleBulletModel(toggle);
        bBulletActive = toggle;

        if (toggle)
        {
            Vector3 aimDirection = focusManager.GetAimDirection();
            bulletMovement.ChangeTravelDirection(aimDirection, GetAimRotation(aimDirection));
            UnparentObject.ObjectUnparented(transform, transform.parent, transform.position);
        }
    }

    public void ToggleBulletPossessed(bool toggle)
    {
        bulletCameraController.ToggleCamera(toggle);
        focusManager.ToggleCanFocus(toggle);
        if (toggle)
        {
            RewindableMovement.UpdateMovementTimescale(1f / bulletMovement.GetVelocity());
        }

        bBulletPossessed = toggle;
    }

    public void SetIsFocusing(bool isFocusing)
    {
        focusManager.ToggleFocusing(isFocusing);
    }

    public bool IsFocusing()
    {
        return focusManager.IsFocusing();
    }

    public void ResetBullet(Transform bulletPosition)
    {
        transform.parent = gunParent;
        transform.position = bulletPosition.position;
    }

    // public bool IsBulletActive()
    // {
    //     return bBulletActive;
    // }

    // public void BulletImpact()
    // {
    //     bulletStateMachine.SwitchState(new BulletDeadState(bulletStateMachine));
    // }

    private void OnCollisionEnter(Collision other)
    {
        if (!bBulletActive)
        {
            return;
        }

        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(out float velocityConservation, out bool bIsPassable);
            bulletMovement.RicochetBullet(other, velocityConservation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bBulletActive)
        {
            return;
        }

        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(out float velocityConservation, out bool bIsPassable);

            bulletMovement.SlowBullet(velocityConservation);
        }
    }
}
