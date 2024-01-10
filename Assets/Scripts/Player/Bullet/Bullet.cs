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
        focusManager = GetComponent<FocusManager>();
    }

    private void Start()
    {
        redirectManager = RedirectManager.Instance;
    }

    // private void OnDisable()
    // {
    //     InputManager.Instance.OnShootAction -= RedirectBullet;
    // }

    public void RedirectBullet()
    {
        if (!redirectManager)
        {
            Debug.Log("No Redirect Manager Instance!");
            return;
        }

        if (focusManager.IsFocusing() && redirectManager.TryRedirect())
        {
            Redirect.BulletRedirected(
                transform.position,
                bulletMovement.GetFlightDirection(),
                this
            );
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

    public void UndoRedirect(Vector3 position, Vector3 direction)
    {
        Quaternion undoRotation = Quaternion.LookRotation(direction);
        bulletMovement.RedirectBullet(direction, undoRotation);
        transform.position = position;
        redirectManager.IncrementRedirects();
    }

    //Should be split into making the bullet active / taking control of bullet
    //Active bullet moves, controlled bullet toggles camera + focus can focus
    //+ subbing to OnShoot
    public void ToggleBulletActive(bool toggle)
    {
        bulletMovement.ToggleMovement(toggle);
        bBulletActive = toggle;

        if (toggle)
        {
            bulletMovement.RedirectBullet(focusManager.GetAimDirection(), GetAimRotation());
        }

        //bBulletActive = toggle;
        // if (toggle)
        // {
        //     InputManager.Instance.OnShootAction += RedirectBullet;

        //     //Refactor needed

        // }
        // else
        // {
        //     InputManager.Instance.OnShootAction -= RedirectBullet;
        // }
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
            damageable.ProjectileHit(this);
        }
    }
}
