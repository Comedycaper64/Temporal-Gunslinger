using System;
using Cinemachine;
using UnityEngine;

using Random = UnityEngine.Random;

public class BulletMovement : RewindableMovement
{
    private bool bIsDead;
    private bool bShouldRotate;
    private bool bLowVelocity = false;

    [SerializeField]
    private bool bShouldSpin = true;

    [SerializeField]
    private bool bDisableModelWhenInactive = true;
    private float rotationTimer;
    private float rotationSpeed = 2.5f;
    private float spinSpeedModifier = 200f;
    private float ricochetRaycastDistance = 1f;
    private float colliderRadius = 0.025f;

    [SerializeField]
    private float lowVelocityThreshhold = 50f;

    [SerializeField]
    private float dropVelocityThreshhold = 25f;
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
    private Sprite pocketwatchDangerSprite;

    [SerializeField]
    private GameObject redirectCoinPrefab;

    private CinemachineImpulseSource cinemachineImpulseSource;
    private RedirectManager redirectManager;

    public EventHandler<bool> OnLowVelocity;
    public static EventHandler<PocketwatchDanger> OnChangeDirection;
    public Action OnRedirect;
    public Action OnRicochet;
    public Action OnSlowed;
    public Action OnMovementStopped;

    private void Start()
    {
        redirectManager = RedirectManager.Instance;
        movementTarget = GameManager.GetRevenant();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        revenantOffset = new Vector3(
            Random.Range(-0.025f, 0.025f),
            Random.Range(-0.025f, 0.025f),
            Random.Range(-0.025f, 0.025f)
        );
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DangerTracker.dangers.Add(this, new PocketwatchDanger(pocketwatchDangerSprite, 9999f));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DangerTracker.dangers.Remove(this);
    }

    protected override void StartMovement()
    {
        SetSpeed(startSpeed);
        movementActive = true;
        targetRotation = bulletModel.rotation;
    }

    private void Update()
    {
        if (bIsDead)
        {
            return;
        }

        transform.position += flightDirection * GetSpeed() * Time.deltaTime;

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

    private void LateUpdate()
    {
        if (bShouldSpin)
        {
            SpinBullet();
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

    public bool TryRedirect()
    {
        if (redirectManager.TryRedirect())
        {
            return true;
        }
        else
        {
            AudioManager.PlaySFX(noCoinsSFX, 0.4f, 5, transform.position);
            return false;
        }
    }

    public bool CanRedirect() => redirectManager.CanRedirect();

    public void RedirectBullet(Vector3 newDirection, Quaternion newRotation)
    {
        if (TryRedirect())
        {
            RedirectManager.SpawnRedirectCoin(bulletTip.position);

            cinemachineImpulseSource.GenerateImpulse();

            Redirect.BulletRedirected(transform.position, GetFlightDirection(), this, 1f, false);
            int randomPitch = Random.Range(2, 5);
            AudioManager.PlaySFX(redirectSFX, 0.2f, randomPitch, transform.position);

            RicochetManager.SpawnRicochetVFX(
                transform,
                Quaternion.LookRotation(GetFlightDirection())
            );

            ChangeTravelDirection(newDirection, newRotation);

            OnRedirect?.Invoke();
        }
    }

    public void ChangeTravelDirection(Vector3 newDirection, Quaternion newRotation)
    {
        flightDirection = newDirection;

        if (!bShouldRotate)
        {
            rotationTimer = 0f;
        }

        if (Quaternion.Angle(targetRotation, newRotation) > 0.1f)
        {
            bShouldRotate = true;
            targetRotation = newRotation;
        }

        float deathTime;
        WillKillRevenant(out deathTime);
        OnChangeDirection?.Invoke(this, new PocketwatchDanger(pocketwatchDangerSprite, deathTime));
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
        Vector3 backUpNormal;

        if (
            Physics.Raycast(
                transform.position,
                GetFlightDirection(),
                out RaycastHit hit,
                ricochetRaycastDistance,
                ricochetLayermask
            )
        )
        {
            hitNormal = hit.normal.normalized;
            backUpNormal = hitObject.GetContact(0).normal.normalized;
            //Debug.Log("Contact Normal: " + hitObject.GetContact(0).normal);

            if (hitObject.collider != hit.collider)
            {
                RaycastHit[] allHits = Physics.SphereCastAll(
                    transform.position - (flightDirection * 0.1f),
                    colliderRadius,
                    GetFlightDirection(),
                    ricochetRaycastDistance,
                    ricochetLayermask
                );

                hitNormal = backUpNormal;

                foreach (RaycastHit newHit in allHits)
                {
                    if (hitObject.collider == newHit.collider)
                    {
                        hitNormal = newHit.normal.normalized;
                    }
                }
            }
        }
        else
        {
            hitNormal = hitObject.GetContact(0).normal.normalized;
        }

        //Debug.Log("Hit object: " + hitObject.gameObject.name);

        Vector3 flightNormalized = GetFlightDirection().normalized;

        Vector3 ricochetDirection = Vector3.Reflect(flightNormalized, hitNormal);

        //Debug.Log("Ricochet Direction: " + ricochetDirection);

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

        OnRicochet?.Invoke();
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
        OnSlowed?.Invoke();
    }

    public void LoseVelocity()
    {
        float speed = GetUnscaledSpeed();
        SetSpeed(speed -= velocityLossRate * GetTimescale() * Time.deltaTime);

        if (!bLowVelocity && ShouldBulletDrop())
        {
            bLowVelocity = true;
            OnLowVelocity?.Invoke(this, true);
        }
        else if (bLowVelocity && !ShouldBulletDrop())
        {
            bLowVelocity = false;
            OnLowVelocity?.Invoke(this, false);
        }
    }

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

    public void SetColliderRadius(float radius)
    {
        colliderRadius = radius + (radius * 0.25f);
    }

    public void SetIsDead(bool isDead)
    {
        bIsDead = isDead;

        float deathTime = -1f;

        if (!isDead)
        {
            WillKillRevenant(out deathTime);
        }
        else
        {
            OnMovementStopped?.Invoke();
        }

        OnChangeDirection?.Invoke(this, new PocketwatchDanger(pocketwatchDangerSprite, deathTime));
    }

    public override void ToggleMovement(bool toggle)
    {
        base.ToggleMovement(toggle);

        float deathTime = -1f;

        if (toggle)
        {
            WillKillRevenant(out deathTime);
        }

        OnChangeDirection?.Invoke(this, new PocketwatchDanger(pocketwatchDangerSprite, deathTime));
    }

    private bool WillKillRevenant(out float deathTime)
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
            if (hit.transform.CompareTag("Revenant"))
            {
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

    public float GetLowVelocity()
    {
        return lowVelocityThreshhold;
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
