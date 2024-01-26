using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private bool bBulletActive;
    private bool bBulletPossessed;
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

    private void Update()
    {
        if (bBulletActive)
        {
            bulletMovement.LoseVelocity();
        }

        if (bBulletPossessed)
        {
            BulletVelocityUI.Instance.VelocityChanged(bulletMovement.GetVelocity());
            TimeManager.UpdateTimeScale(1f / bulletMovement.GetVelocity());
        }
    }

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

    // public bool IsBulletActive()
    // {
    //     return bBulletActive;
    // }

    public void BulletImpact()
    {
        bulletStateMachine.SwitchState(new BulletDeadState(bulletStateMachine));
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ayaya");

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
        Debug.Log("bayaya");

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
