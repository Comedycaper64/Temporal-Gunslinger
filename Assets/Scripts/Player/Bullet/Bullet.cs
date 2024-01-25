using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private bool bBulletActive;
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
    }

    // private void OnDisable()
    // {
    //     InputManager.Instance.OnShootAction -= RedirectBullet;
    // }

    public void RedirectBullet()
    {
        if (focusManager.IsFocusing())
        {
            bulletMovement.RedirectBullet(focusManager.GetAimDirection(), GetAimRotation());
        }
        else
        {
            //Sound effect or other indicator
        }
    }

    private Quaternion GetAimRotation()
    {
        return Quaternion.LookRotation(focusManager.GetAimDirection());
    }

    public void ToggleBulletActive(bool toggle)
    {
        bulletMovement.ToggleMovement(toggle);
        bBulletActive = toggle;

        if (toggle)
        {
            bulletMovement.ChangeTravelDirection(focusManager.GetAimDirection(), GetAimRotation());
            UnparentObject.ObjectUnparented(transform, transform.parent);
        }
    }

    public void ToggleBulletPossessed(bool toggle)
    {
        bulletCameraController.ToggleCamera(toggle);
        focusManager.ToggleCanFocus(toggle);
    }

    public void SetIsFocusing(bool isFocusing)
    {
        focusManager.ToggleFocusing(isFocusing);
    }

    public bool IsFocusing()
    {
        return focusManager.IsFocusing();
    }

    // public bool IsBulletActive()
    // {
    //     return bBulletActive;
    // }

    public void BulletImpact()
    {
        bulletStateMachine.SwitchState(new BulletDeadState(bulletStateMachine));
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
            if (!bIsPassable)
            {
                bulletMovement.RicochetBullet(other, velocityConservation);
            }
            else
            {
                bulletMovement.SlowBullet(velocityConservation);
            }
        }
    }
}
