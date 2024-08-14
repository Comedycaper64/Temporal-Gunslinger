using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using Random = UnityEngine.Random;

public class BulletMovement : RewindableMovement
{
    private bool bIsDead;

    //private bool deadFlag = false;
    private bool bShouldRotate;
    private bool bLowVelocity = false;

    [SerializeField]
    private bool bShouldSpin = true;

    [SerializeField]
    private bool bDisableModelWhenInactive = true;

    private float rotationTimer;
    private float rotationSpeed = 2.5f;

    private float spinSpeedModifier = 200f;

    [SerializeField]
    private float lowVelocityThreshhold = 50f;

    [SerializeField]
    private float dropVelocityThreshhold = 25f;

    //private float dropLerpValue = 0f;
    public float velocityLossRate = 5f;

    private Vector3 flightDirection;

    private Vector3 revenantOffset;
    private Quaternion targetRotation;

    [SerializeField]
    private LayerMask ricochetLayermask;

    [SerializeField]
    private LayerMask revenantLayermask;

    [SerializeField]
    private Transform bulletModel;

    [SerializeField]
    private Transform bulletTip;

    // [SerializeField]
    // private TrailRenderer[] activeTrails;

    [SerializeField]
    private Transform damagePoint;
    private Transform movementTarget;

    [SerializeField]
    private AudioClip redirectSFX;

    [SerializeField]
    private AudioClip noCoinsSFX;

    [SerializeField]
    private AudioClip[] ricochetSFX;

    [SerializeField]
    private GameObject redirectCoinPrefab;

    [SerializeField]
    private GameObject redirectVFXPrefab;
    private CinemachineImpulseSource cinemachineImpulseSource;

    private RedirectManager redirectManager;

    public EventHandler<bool> OnLowVelocity;

    public static Action OnRedirect;

    private void Start()
    {
        redirectManager = RedirectManager.Instance;
        movementTarget = GameManager.GetRevenant();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        revenantOffset = new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f)
        );
        //lowVelocityThreshhold = startSpeed / 10f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DangerTracker.dangers.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DangerTracker.dangers.Remove(this);
    }

    // protected override void StartMovement()
    // {
    //     base.StartMovement();

    //     Debug.Log("Bullet Movement Started: " + gameObject.name);

    //     if (!moveToTarget)
    //     {
    //         Vector3 aimDirection = focusManager.GetAimDirection();
    //         ChangeTravelDirection(aimDirection, Quaternion.LookRotation(aimDirection));
    //     }
    //     else
    //     {
    //         Vector3 target = movementTarget.position + revenantOffset;
    //         Vector3 aimDirection = (target - transform.position).normalized;
    //         ChangeTravelDirection(aimDirection, Quaternion.LookRotation(aimDirection));
    //     }
    // }

    protected override void StartMovement()
    {
        // if (!deadFlag)
        // {
        SetSpeed(startSpeed);
        // }
        // else
        // {
        //     SetSpeed(-1f);
        // }

        movementActive = true;
    }

    private void Update()
    {
        if (bIsDead)
        {
            return;
        }

        transform.position += flightDirection * GetSpeed() * Time.deltaTime;

        if (bShouldSpin)
        {
            SpinBullet();
        }

        if (!bShouldRotate)
        {
            return;
        }

        bulletModel.rotation = Quaternion.Slerp(
            bulletModel.rotation,
            targetRotation,
            rotationTimer * rotationSpeed
        );
        rotationTimer += Time.deltaTime;

        if (Quaternion.Angle(bulletModel.rotation, targetRotation) < 0.1f)
        {
            bShouldRotate = false;
        }
    }

    private void SpinBullet()
    {
        bulletModel.eulerAngles += new Vector3(
            0f,
            0f,
            GetStartSpeed() * GetTimescale() * spinSpeedModifier * Time.deltaTime
        );
    }

    public void RedirectBullet(Vector3 newDirection, Quaternion newRotation)
    {
        if (redirectManager.TryRedirect())
        {
            GameObject coin = Factory.InstantiateGameObject(
                redirectCoinPrefab,
                bulletTip.position,
                Quaternion.identity
            );

            cinemachineImpulseSource.GenerateImpulse();

            coin.transform.eulerAngles = new Vector3(0f, coin.transform.eulerAngles.y, 0f);

            Redirect.BulletRedirected(transform.position, GetFlightDirection(), this, 1f, false);
            int randomPitch = Random.Range(2, 5);
            AudioManager.PlaySFX(redirectSFX, 0.2f, randomPitch, transform.position);

            Factory.InstantiateGameObject(
                redirectVFXPrefab,
                transform.position,
                Quaternion.LookRotation(GetFlightDirection())
            );

            ChangeTravelDirection(newDirection, newRotation);
        }
        else
        {
            AudioManager.PlaySFX(noCoinsSFX, 0.4f, 5, transform.position);
        }
    }

    public void ChangeTravelDirection(Vector3 newDirection, Quaternion newRotation)
    {
        flightDirection = newDirection;
        targetRotation = newRotation;
        rotationTimer = 0f;
        bShouldRotate = true;

        OnRedirect?.Invoke();
    }

    public Vector3 GetRevenantDirection()
    {
        Vector3 target = movementTarget.position + revenantOffset;
        Vector3 aimDirection = (target - transform.position).normalized;
        return aimDirection;
    }

    public void RicochetBullet(Collision hitObject, float velocityAugment)
    {
        Vector3 hitNormal;

        if (
            Physics.Raycast(
                transform.position,
                GetFlightDirection(),
                out RaycastHit hit,
                1f,
                ricochetLayermask
            )
        )
        {
            hitNormal = hit.normal.normalized;
        }
        else
        {
            hitNormal = hitObject.GetContact(0).normal.normalized;
        }

        Vector3 flightNormalized = GetFlightDirection().normalized;

        Vector3 ricochetDirection = Vector3.Reflect(flightNormalized, hitNormal);

        int randomIndex = Random.Range(0, ricochetSFX.Length);

        AudioManager.PlaySFX(ricochetSFX[randomIndex], 0.4f, 0, transform.position);

        Redirect.BulletRedirected(
            transform.position,
            GetFlightDirection(),
            this,
            velocityAugment,
            true
        );

        AugmentVelocity(velocityAugment);
        ChangeTravelDirection(ricochetDirection, Quaternion.LookRotation(ricochetDirection));
    }

    public void SlowBullet(float velocityAugment)
    {
        Redirect.BulletRedirected(
            transform.position,
            GetFlightDirection(),
            this,
            velocityAugment,
            true
        );

        AugmentVelocity(velocityAugment);
    }

    public Vector3 GetFlightDirection()
    {
        return flightDirection;
    }

    public float GetVelocity()
    {
        //return Mathf.Abs(GetUnscaledSpeed());
        float velocity = GetUnscaledSpeed();

        // if (bIsDead)
        // {
        //     return 0f;
        // }
        // else
        // {
        if (velocity == 0f)
        {
            return Mathf.Abs(GetStartSpeed());
        }
        else
        {
            return Mathf.Abs(GetUnscaledSpeed());
        }
        // }
    }

    public float GetMaxVelocity()
    {
        return Mathf.Abs(GetStartSpeed());
    }

    public void AugmentVelocity(float velocityMultiplier)
    {
        float speed = GetUnscaledSpeed();
        SetSpeed(speed *= velocityMultiplier);
        //rewindable action for undoing augmention
    }

    public void LoseVelocity()
    {
        float speed = GetUnscaledSpeed();
        SetSpeed(speed -= velocityLossRate * GetTimescale() * Time.deltaTime);

        if (!bLowVelocity && ShouldBulletDrop())
        {
            //SetLowVelocity(true);
            bLowVelocity = true;
            OnLowVelocity?.Invoke(this, true);
        }
        else if (bLowVelocity && !ShouldBulletDrop())
        {
            //SetLowVelocity(false);
            bLowVelocity = false;
            OnLowVelocity?.Invoke(this, false);
        }
    }

    // public void SetLowVelocity(bool toggle)
    // {
    //     bLowVelocity = toggle;
    //     Debug.Log("Low Velocity " + toggle);
    //     if (toggle)
    //     {
    //         bulletDissolve.StartDissolve(0.33f);
    //     }
    //     else
    //     {
    //         bulletDissolve.StopDissolve();
    //     }

    //     //Show warning on UI
    // }

    public bool ShouldBulletDrop()
    {
        return Mathf.Abs(GetUnscaledSpeed()) < lowVelocityThreshhold;
    }

    public bool ShouldBulletStop()
    {
        return Mathf.Abs(GetUnscaledSpeed()) < dropVelocityThreshhold;
    }

    public void ToggleBulletModel(bool toggle)
    {
        if (bDisableModelWhenInactive)
        {
            bulletModel.gameObject.SetActive(toggle);
        }
    }

    public bool IsBulletReversing()
    {
        return GetUnscaledSpeed() < 0f;
    }

    public void SetIsDead(bool isDead)
    {
        bIsDead = isDead;
        // if (bIsDead)
        // {
        //     deadFlag = true;
        // }
    }

    // public void RemoveDeadFlag()
    // {
    //     deadFlag = false;
    // }

    public bool WillKillRevenant(out float deathTime)
    {
        if (
            Physics.Raycast(
                transform.position,
                GetFlightDirection(),
                out RaycastHit hit,
                500f,
                revenantLayermask
            )
        )
        {
            if (hit.transform.CompareTag("Player"))
            {
                //Debug.Log("ayaya");
                deathTime = GetTimeToRevenant();
                return true;
            }
        }

        deathTime = -1f;
        return false;
    }

    private float GetTimeToRevenant()
    {
        float distanceToTarget = Vector3.Distance(
            damagePoint.position,
            movementTarget.position + new Vector3(0f, 1.4f, 0f)
        );
        float timeToRevenant =
            (
                -Mathf.Abs(GetUnscaledSpeed())
                + Mathf.Sqrt(
                    Mathf.Pow(Mathf.Abs(GetUnscaledSpeed()), 2f)
                        - 4f * (-0.5f * velocityLossRate * -distanceToTarget)
                )
            ) / (-velocityLossRate);

        return timeToRevenant;
    }

    // public override void BeginPlay()
    // {
    //     base.BeginPlay();

    //     foreach (TrailRenderer trail in activeTrails) { }
    // }

    // public override void BeginRewind()
    // {
    //     base.BeginRewind();

    //     foreach (TrailRenderer trail in activeTrails) { }
    // }

    public bool GetIsLowVelocity()
    {
        return bLowVelocity;
    }

    public void UndoRedirect(
        Vector3 position,
        Vector3 direction,
        float velocityAugment,
        bool bIsRicochet
    )
    {
        Quaternion undoRotation = Quaternion.LookRotation(direction);
        ChangeTravelDirection(direction, undoRotation);
        transform.position = position;
        float speed = GetUnscaledSpeed();
        SetSpeed(speed /= velocityAugment);
        if (!bIsRicochet)
        {
            redirectManager.IncrementRedirects();
        }
    }
}
