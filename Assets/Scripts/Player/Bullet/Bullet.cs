using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private bool bBulletActive;
    private bool bBulletPossessed;
    private Transform gunParent;
    private AudioSource bulletFlightSFX;
    private BulletMovement bulletMovement;

    [SerializeField]
    private BulletDamager bulletDamager;
    private BulletCameraController bulletCameraController;
    private BulletStateMachine bulletStateMachine;
    private FocusManager focusManager;

    [SerializeField]
    private AudioClip[] possessSFX;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletCameraController = GetComponent<BulletCameraController>();
        bulletStateMachine = GetComponent<BulletStateMachine>();
        focusManager = GetComponent<FocusManager>();
        bulletFlightSFX = GetComponent<AudioSource>();
        gunParent = transform.parent;
    }

    private void Update()
    {
        if (bBulletActive)
        {
            bulletMovement.LoseVelocity();

            if (bulletMovement.ShouldBulletDrop())
            {
                //bulletStateMachine.SwitchToDeadState();
                bulletMovement.ApplyGravity();
            }

            if (bulletMovement.ShouldBulletStop())
            {
                bulletStateMachine.SwitchToDeadState();
            }
        }

        if (bBulletPossessed)
        {
            BulletVelocityUI.Instance.VelocityChanged(
                bulletMovement.GetVelocity(),
                bulletMovement.GetMaxVelocity()
            );
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
    }

    private Quaternion GetAimRotation(Vector3 aimDirection)
    {
        return Quaternion.LookRotation(aimDirection);
    }

    public void ToggleBulletActive(bool toggle)
    {
        bulletMovement.ToggleMovement(toggle);
        bulletMovement.ToggleBulletModel(toggle);
        bulletDamager.SetBulletActive(toggle);
        bBulletActive = toggle;

        if (toggle)
        {
            Vector3 aimDirection = focusManager.GetAimDirection();
            bulletMovement.ChangeTravelDirection(aimDirection, GetAimRotation(aimDirection));
            UnparentObject.ObjectUnparented(transform, transform.parent, transform.position);
            bulletFlightSFX.Play();
        }
        else
        {
            bulletFlightSFX.Stop();
        }
    }

    public void ToggleBulletPossessed(bool toggle)
    {
        bulletCameraController.ToggleCamera(toggle);
        focusManager.ToggleCanFocus(toggle);
        //Debug.Log(gameObject.name + " " + toggle);
        if (toggle)
        {
            RewindableMovement.UpdateMovementTimescale(1f / bulletMovement.GetVelocity());
            int randomInt = Random.Range(0, possessSFX.Length);
            AudioManager.PlaySFX(possessSFX[randomInt], 0.25f, 4, transform.position);
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
        bulletStateMachine.SwitchToInactive();
        ToggleBulletPossessed(false);
        //bulletMovement.ResetMovement();
        transform.parent = gunParent;
        transform.position = bulletPosition.position;
        transform.rotation = bulletPosition.rotation;
    }

    // public bool IsBulletActive()
    // {
    //     return bBulletActive;
    // }

    // public void BulletImpact()
    // {
    //     bulletStateMachine.SwitchState(new BulletDeadState(bulletStateMachine));
    // }
}
