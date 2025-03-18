using System;
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

    [SerializeField]
    private bool followWhenInactive = true;
    private Transform gunParent;
    private AudioSource bulletFlightSFX;
    private BulletMovement bulletMovement;
    private BulletLockOn bulletLockOn;

    [SerializeField]
    private BulletDamager bulletDamager;
    private BulletCameraController bulletCameraController;
    private BulletStateMachine bulletStateMachine;
    private FocusManager focusManager;

    [SerializeField]
    private AudioClip[] possessSFX;
    public Action OnShuntOut;

    public EventHandler<bool> OnActiveToggled;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletLockOn = GetComponent<BulletLockOn>();
        bulletCameraController = GetComponent<BulletCameraController>();
        bulletStateMachine = GetComponent<BulletStateMachine>();
        focusManager = GetComponent<FocusManager>();
        bulletFlightSFX = GetComponent<AudioSource>();
        gunParent = transform.parent;

        bulletMovement.OnLowVelocity += SetLowVelocity;
    }

    private void OnDisable()
    {
        bulletMovement.OnLowVelocity -= SetLowVelocity;
    }

    private void Update()
    {
        if (bIsDead)
        {
            return;
        }

        if (bBulletActive)
        {
            bulletMovement.LoseVelocity();

            if (bulletMovement.ShouldBulletStop() && !RewindManager.bRewinding)
            {
                bulletStateMachine.SwitchToDeadState();
            }
        }

        if (bBulletPossessed)
        {
            BulletVelocityUI.Instance.VelocityChanged(
                bulletMovement.GetVelocity(),
                bulletMovement.GetMaxVelocity(),
                bulletMovement.GetLowVelocity()
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

    public bool ToggleLockOn(bool toggle)
    {
        if (!bulletMovement.CanRedirect() && (toggle == true))
        {
            return false;
        }

        return bulletLockOn.ToggleLockOn(toggle);
    }

    public void LockOnBullet()
    {
        if (bulletMovement.CanRedirect())
        {
            bulletLockOn.TryLockOn();
        }
    }

    private Quaternion GetAimRotation(Vector3 aimDirection)
    {
        return Quaternion.LookRotation(aimDirection);
    }

    private void SetLowVelocity(object sender, bool toggle)
    {
        //Debug.Log("Low Velocity " + toggle);
        if (toggle)
        {
            bulletStateMachine.GetDissolveController().StartDissolve(0.33f);
        }
        else
        {
            bulletStateMachine.GetDissolveController().StopDissolve();
        }
    }

    public void ToggleBulletActive(bool toggle)
    {
        if (bBulletActive && toggle)
        {
            return;
        }

        //Debug.Log("Projectile: " + gameObject.name + toggle);

        bulletMovement.ToggleMovement(toggle);
        bulletMovement.ToggleBulletModel(toggle);
        //bulletMovement.RemoveDeadFlag();
        bulletDamager.SetBulletActive(toggle);
        bBulletActive = toggle;
        focusManager.ToggleAimLine(toggle);

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

    public void ToggleBulletPossessed(bool toggle, Vector2 bulletCameraAxis)
    {
        bulletCameraController.ToggleCamera(toggle);
        focusManager.ToggleCanFocus(toggle);

        if (toggle)
        {
            bulletCameraController.SetCameraAxisValues(bulletCameraAxis);

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

    public void SetIsDead(bool isDead, bool shuntOut)
    {
        if (shuntOut && bBulletPossessed)
        {
            //Debug.Log("Shunt!");
            OnShuntOut?.Invoke();
        }

        OnActiveToggled?.Invoke(this, !isDead);

        bIsDead = isDead;
        bulletMovement.SetIsDead(bIsDead);
        bulletDamager.SetBulletActive(!bIsDead);
        focusManager.ToggleAimLine(!bIsDead);
    }

    public bool IsFocusing()
    {
        return focusManager.IsFocusing();
    }

    public Vector2 GetCameraAxisValues() => bulletCameraController.GetCameraAxisValues();

    public Transform GetCameraTransform() => bulletCameraController.GetCameraTransform();

    public void KillBullet() => bulletStateMachine.SwitchToDeadState();

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

        if (!followWhenInactive && bBulletPossessed)
        {
            OnShuntOut?.Invoke();
        }

        ToggleBulletPossessed(false, Vector2.zero);

        transform.parent = gunParent;
        transform.position = bulletPosition.position;
        transform.rotation = bulletPosition.rotation;
    }
}
