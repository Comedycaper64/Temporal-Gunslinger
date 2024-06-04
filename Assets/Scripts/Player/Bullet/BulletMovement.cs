using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BulletMovement : RewindableMovement
{
    private bool bShouldRotate;

    [SerializeField]
    private bool bShouldSpin = true;
    private float rotationTimer;
    private float rotationSpeed = 2.5f;

    private float spinSpeedModifier = 200f;
    private float dropVelocity = 0f;
    public float velocityLossRate = 5f;
    private Vector3 flightDirection;
    private Quaternion targetRotation;

    [SerializeField]
    private LayerMask ricochetLayermask;

    [SerializeField]
    private LayerMask revenantLayermask;

    [SerializeField]
    private Transform bulletModel;

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
    private GameObject redirectVFXPrefab;

    private RedirectManager redirectManager;

    public static Action OnRedirect;

    private void Start()
    {
        redirectManager = RedirectManager.Instance;
        movementTarget = GameManager.GetRevenant();
        dropVelocity = startSpeed / 10f;
        DangerTracker.dangers.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DangerTracker.dangers.Remove(this);
    }

    private void Update()
    {
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
            Redirect.BulletRedirected(transform.position, GetFlightDirection(), this, 1f, false);
            AudioManager.PlaySFX(redirectSFX, 0.4f, 3, transform.position);
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
        if (velocity == 0f)
        {
            return Mathf.Abs(GetStartSpeed());
        }
        else
        {
            return Mathf.Abs(GetUnscaledSpeed());
        }
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
    }

    public void ApplyGravity()
    {
        if (transform.position.y <= 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            return;
        }

        transform.position += new Vector3(0f, -GetSpeed() * Time.deltaTime, 0f);
    }

    public bool ShouldBulletDrop()
    {
        return Mathf.Abs(GetUnscaledSpeed()) < dropVelocity;
    }

    public bool ShouldBulletStop()
    {
        return Mathf.Abs(GetUnscaledSpeed()) < 10f;
    }

    public void ToggleBulletModel(bool toggle)
    {
        bulletModel.gameObject.SetActive(toggle);
    }

    public bool IsBulletReversing()
    {
        return GetUnscaledSpeed() < 0f;
    }

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
        float distanceToTarget = Vector3.Distance(damagePoint.position, movementTarget.position);
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
