using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private bool bIsDead;
    private bool bBulletActive;
    private bool bBulletPossessed;

    [SerializeField]
    private bool moveToTarget = true;
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

    public EventHandler<bool> OnActiveToggled;

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

            if (bulletMovement.ShouldBulletStop())
            {
                bulletStateMachine.SwitchToDeadState();
            }
            else
            {
                if (bulletMovement.ShouldBulletDrop())
                {
                    bulletMovement.ApplyGravity();
                }
            }
        }

        if (bBulletPossessed)
        {
            BulletVelocityUI.Instance.VelocityChanged(
                bulletMovement.GetVelocity(),
                bulletMovement.GetMaxVelocity()
            );

            float bulletMovementVelocity = Mathf.Clamp(bulletMovement.GetVelocity(), 1f, 999f);
            float newMovementTimescale = Mathf.Clamp(1f / bulletMovementVelocity, 0f, 0.1f);
            RewindableMovement.UpdateMovementTimescale(newMovementTimescale);
        }
    }

    public void RedirectBullet()
    {
        if (focusManager.IsFocusing() && bBulletActive)
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
        //Debug.Log("Projectile: " + gameObject.name + toggle);

        bulletMovement.ToggleMovement(toggle);
        bulletMovement.ToggleBulletModel(toggle);
        bulletMovement.RemoveDeadFlag();
        bulletDamager.SetBulletActive(toggle);
        bBulletActive = toggle;

        OnActiveToggled?.Invoke(this, bBulletActive);

        if (toggle)
        {
            if (moveToTarget)
            {
                Vector3 travelDirection = bulletMovement.GetRevenantDirection();
                bulletMovement.ChangeTravelDirection(
                    travelDirection,
                    GetAimRotation(travelDirection)
                );
            }
            else
            {
                Vector3 aimDirection = focusManager.GetAimDirection();
                bulletMovement.ChangeTravelDirection(aimDirection, GetAimRotation(aimDirection));
            }

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

    public void SetIsDead(bool isDead)
    {
        bIsDead = isDead;
        bulletMovement.SetIsDead(bIsDead);
    }

    public bool IsFocusing()
    {
        return focusManager.IsFocusing();
    }

    public void SetFiringPosition(Transform firingPosition)
    {
        transform.position = firingPosition.position;
        transform.rotation = firingPosition.rotation;
    }

    public void ResetBullet(Transform bulletPosition)
    {
        // if (bulletStateMachine)
        // {
        bulletStateMachine.SwitchToInactive();
        // }
        // else
        // {
        //     Debug.Log("Bullet State Machine not found for " + gameObject.name);
        // }

        ToggleBulletPossessed(false);

        transform.parent = gunParent;
        transform.position = bulletPosition.position;
        transform.rotation = bulletPosition.rotation;
    }
}
