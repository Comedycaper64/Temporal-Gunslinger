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
    private RedirectManager redirectManager;
    private FocusManager focusManager;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletCameraController = GetComponent<BulletCameraController>();
        bulletStateMachine = GetComponent<BulletStateMachine>();
        redirectManager = GetComponent<RedirectManager>();
        focusManager = GetComponent<FocusManager>();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnShootAction -= RedirectBullet;
    }

    private void RedirectBullet()
    {
        if (redirectManager.TryRedirect())
        {
            Redirect.BulletRedirected(transform.position, bulletMovement.GetBulletRotation(), this);
            bulletMovement.RedirectBullet(bulletCameraController.GetCameraRotation());
        }
        else
        {
            //Sound effect or other indicator
        }
    }

    public void UndoRedirect(Vector3 position, Quaternion rotation)
    {
        bulletMovement.RedirectBullet(rotation);
        transform.position = position;
        redirectManager.IncrementRedirects();
    }

    public void ToggleBulletActive(bool toggle)
    {
        bulletCameraController.ToggleCamera(toggle);
        bulletMovement.ToggleBulletMovement(toggle);
        focusManager.ToggleCanFocus(toggle);
        bBulletActive = toggle;
        if (toggle)
        {
            InputManager.Instance.OnShootAction += RedirectBullet;
        }
        else
        {
            InputManager.Instance.OnShootAction -= RedirectBullet;
        }
    }

    public bool IsBulletActive()
    {
        return bBulletActive;
    }

    public void BulletImpact()
    {
        bulletStateMachine.SwitchState(new BulletDeadState(bulletStateMachine));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(this);
        }
    }
}
