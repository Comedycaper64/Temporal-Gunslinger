using System;
using UnityEngine;

public class BulletLockOn : MonoBehaviour
{
    private float rotationSpeed = 5f;
    private float currentLockOnTotalRotation = 0f;
    private Vector3 initialDirection;

    private BulletMovement bulletMovement;
    private LockOnTarget bulletTarget;
    private LockOnTarget lockingOnTarget;

    [SerializeField]
    private LayerMask lockOnLayermask;

    public static EventHandler<bool> OnLockOnUI;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
    }

    private void OnEnable()
    {
        bulletMovement.OnRedirect += DisableLockOn;
        bulletMovement.OnRicochet += DisableLockOn;
    }

    private void OnDisable()
    {
        bulletMovement.OnRedirect -= DisableLockOn;
        bulletMovement.OnRicochet -= DisableLockOn;
    }

    private void Update()
    {
        if (!bulletTarget)
        {
            return;
        }

        AlterTrajectory();
    }

    private void AlterTrajectory()
    {
        Vector3 targetPosition = bulletTarget.GetTarget().position;

        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        float singleStep = rotationSpeed * Time.deltaTime * RewindableMovement.GetTimescale(); // * rewindModifier;

        Vector3 currentDirection = bulletMovement.GetFlightDirection();

        Vector3 newDirection;

        float rewindModifier = 1f;

        if (bulletMovement.IsBulletReversing())
        {
            rewindModifier = -1f;
            newDirection = Vector3.RotateTowards(
                currentDirection,
                initialDirection,
                singleStep,
                singleStep
            );
        }
        else
        {
            newDirection = Vector3.RotateTowards(
                currentDirection,
                targetDirection,
                singleStep,
                singleStep
            );
        }

        float distanceRotated = (newDirection - currentDirection).magnitude;

        // Debug.Log(
        //     "New Direction: "
        //         + newDirection
        //         + ", Current Direction: "
        //         + currentDirection
        //         + ", Distance Rotated: "
        //         + distanceRotated
        // );

        currentLockOnTotalRotation += distanceRotated * rewindModifier;

        //Debug.Log("Current LockOn Rotation: " + currentLockOnTotalRotation);

        if (currentLockOnTotalRotation > 0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(newDirection);
            bulletMovement.ChangeTravelDirection(newDirection, newRotation);
        }
        else
        {
            //Debug.Log("No More Rotation");
            currentLockOnTotalRotation = 0f;
        }
    }

    private void DisableLockOn()
    {
        SetTarget(null, false);
    }

    public void ToggleLockOn(bool toggle)
    {
        if (toggle)
        {
            if (
                Physics.Raycast(
                    transform.position,
                    Camera.main.transform.forward,
                    out RaycastHit hit,
                    500f,
                    lockOnLayermask
                )
            )
            {
                if (hit.transform.TryGetComponent<LockOnTarget>(out LockOnTarget lockOnTarget))
                {
                    lockingOnTarget = lockOnTarget;
                    OnLockOnUI?.Invoke(this, true);
                    //Turn on lockon UI;
                }
            }
        }
        else
        {
            lockingOnTarget = null;
            OnLockOnUI?.Invoke(this, false);
            //Turn off lockon UI;
        }
    }

    public void TryLockOn()
    {
        //Might need a check to see if bullet is active?

        if (!lockingOnTarget)
        {
            return;
        }

        Vector3 redirectDirection = (
            lockingOnTarget.GetTarget().position - transform.position
        ).normalized;

        bulletMovement.RedirectBullet(
            redirectDirection,
            Quaternion.LookRotation(redirectDirection)
        );

        Debug.Log("Locking On");

        SetTarget(lockingOnTarget, false);

        //Turn off lockon UI;
        OnLockOnUI?.Invoke(this, false);
        lockingOnTarget = null;
    }

    public void SetTarget(LockOnTarget newTarget, bool rewinding)
    {
        if (bulletTarget != null)
        {
            bulletTarget.OnTargetDestroyed -= DisableLockOn;

            if (!rewinding)
            {
                FinishLockOn.BulletLockOnFinished(
                    bulletTarget,
                    this,
                    currentLockOnTotalRotation,
                    transform.position,
                    bulletMovement.GetFlightDirection(),
                    initialDirection
                );
            }
        }

        bulletTarget = newTarget;

        if (bulletTarget != null)
        {
            bulletTarget.OnTargetDestroyed += DisableLockOn;
            currentLockOnTotalRotation = 0f;
            initialDirection = bulletMovement.GetFlightDirection();
            if (!rewinding)
            {
                StartLockOn.BulletLockedOn(
                    this,
                    transform.position,
                    bulletMovement.GetFlightDirection()
                );
            }
        }
    }

    public void UndoLockOn(Vector3 initialPosition, Vector3 initialDirection)
    {
        SetTarget(null, true);
        transform.position = initialPosition;
        Quaternion undoRotation = Quaternion.LookRotation(initialDirection);
        bulletMovement.ChangeTravelDirection(initialDirection, undoRotation);
    }

    public void RestartLockOn(
        LockOnTarget oldTarget,
        float lockOnRotationAmount,
        Vector3 initialPosition,
        Vector3 lastDirection,
        Vector3 lockOnStartDirection
    )
    {
        //Debug.Log("Lock On Restarted");
        SetTarget(oldTarget, true);
        currentLockOnTotalRotation = lockOnRotationAmount;
        transform.position = initialPosition;
        initialDirection = lockOnStartDirection;
        // Quaternion undoRotation = Quaternion.LookRotation(lastDirection);
        // bulletMovement.ChangeTravelDirection(lastDirection, undoRotation);
    }
}
