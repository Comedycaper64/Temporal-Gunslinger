using System;
using UnityEngine;

public class BulletLockOn : MonoBehaviour
{
    private bool activelyHoming = false;

    [SerializeField]
    private bool persistentLockOn = true;

    //[SerializeField]
    private bool experimentalLockOn = true;

    [SerializeField]
    private float rotationSpeed = 10f;
    private float reactivateHomingThreshold = 0.02f;
    private float currentLockOnTotalRotation = 0f;
    private Vector3 initialDirection;

    private BulletMovement bulletMovement;
    private LockOnTarget bulletTarget;
    private LockOnTarget lockingOnTarget;

    [SerializeField]
    private LayerMask lockOnLayermask;

    public static EventHandler<bool> OnLockOnUI;
    public static Action OnLockOnFinish;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
    }

    private void OnEnable()
    {
        bulletMovement.OnRedirect += DisableLockOn;
        bulletMovement.OnRicochet += DisableLockOn;
        bulletMovement.OnMovementStopped += DisableLockOn;
    }

    private void OnDisable()
    {
        bulletMovement.OnRedirect -= DisableLockOn;
        bulletMovement.OnRicochet -= DisableLockOn;
        bulletMovement.OnMovementStopped -= DisableLockOn;
    }

    private void FixedUpdate()
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

        //float singleStep = rotationSpeed * Time.deltaTime * RewindableMovement.GetTimescale(); // * rewindModifier;
        //FIXED UPDATE VERSION \/
        float singleStep = rotationSpeed * Time.fixedDeltaTime * RewindableMovement.GetTimescale(); // * rewindModifier;

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

        //currentLockOnTotalRotation += distanceRotated * rewindModifier;
        float lockOnRotationincrease = distanceRotated * rewindModifier;
        //Debug.Log("Current LockOn Rotation: " + currentLockOnTotalRotation);


        if (experimentalLockOn)
        {
            if (activelyHoming)
            {
                currentLockOnTotalRotation += lockOnRotationincrease;
                if (
                    !bulletMovement.IsBulletReversing()
                    && ((targetDirection - currentDirection).magnitude <= 0.01f)
                )
                {
                    activelyHoming = false;
                    FinishLockOn.BulletLockOnFinished(
                        bulletTarget,
                        this,
                        currentLockOnTotalRotation,
                        transform.position,
                        bulletMovement.GetFlightDirection(),
                        initialDirection,
                        true
                    );
                    currentLockOnTotalRotation = 0f;
                    return;
                }

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
                return;
            }
            else
            {
                if (
                    !bulletMovement.IsBulletReversing()
                    && ((targetDirection - currentDirection).magnitude > reactivateHomingThreshold)
                )
                {
                    activelyHoming = true;
                    initialDirection = bulletMovement.GetFlightDirection();
                    StartLockOn.BulletLockedOn(
                        this,
                        transform.position,
                        bulletMovement.GetFlightDirection(),
                        true
                    );
                    return;
                }
            }

            return;
        }

        currentLockOnTotalRotation += lockOnRotationincrease;

        if (
            !persistentLockOn
            && !bulletMovement.IsBulletReversing()
            && ((targetDirection - currentDirection).magnitude <= 0.01f)
        )
        {
            DisableLockOn();
            return;
        }

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

    public bool ToggleLockOn(bool toggle)
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
                    return true;
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
        return false;
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

        SetTarget(lockingOnTarget, false);

        //Turn off lockon UI;
        OnLockOnFinish?.Invoke();
        lockingOnTarget = null;
    }

    private void ResetTarget()
    {
        activelyHoming = true;
    }

    public void SetTarget(LockOnTarget newTarget, bool rewinding)
    {
        if (bulletTarget != null)
        {
            activelyHoming = false;

            bulletTarget.OnTargetDestroyed -= DisableLockOn;

            if (!rewinding)
            {
                FinishLockOn.BulletLockOnFinished(
                    bulletTarget,
                    this,
                    currentLockOnTotalRotation,
                    transform.position,
                    bulletMovement.GetFlightDirection(),
                    initialDirection,
                    false
                );
            }
        }

        bulletTarget = newTarget;
        //Debug.Log("Setting target");
        if (bulletTarget != null)
        {
            activelyHoming = true;

            //Debug.Log("Has target");
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

    public void LockOnOverride(LockOnTarget target)
    {
        SetTarget(target, false);
    }

    public void UndoLockOn(Vector3 initialPosition, Vector3 initialDirection, bool persistent)
    {
        if (!persistent)
        {
            SetTarget(null, true);
        }
        else
        {
            activelyHoming = false;
        }

        transform.position = initialPosition;
        Quaternion undoRotation = Quaternion.LookRotation(initialDirection);
        bulletMovement.ChangeTravelDirection(initialDirection, undoRotation);
    }

    public void RestartLockOn(
        LockOnTarget oldTarget,
        float lockOnRotationAmount,
        Vector3 initialPosition,
        Vector3 lastDirection,
        Vector3 lockOnStartDirection,
        bool persistent
    )
    {
        //Debug.Log("Lock On Restarted");
        if (!persistent)
        {
            SetTarget(oldTarget, true);
        }
        else
        {
            ResetTarget();
        }
        currentLockOnTotalRotation = lockOnRotationAmount;
        transform.position = initialPosition;

        Quaternion undoRotation = Quaternion.LookRotation(lastDirection);
        bulletMovement.ChangeTravelDirection(lastDirection, undoRotation);
        initialDirection = lockOnStartDirection;
        // Quaternion undoRotation = Quaternion.LookRotation(lastDirection);
        // bulletMovement.ChangeTravelDirection(lastDirection, undoRotation);
    }
}
