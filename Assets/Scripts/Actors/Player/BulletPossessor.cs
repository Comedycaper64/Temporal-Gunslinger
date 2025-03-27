using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessor : MonoBehaviour
{
    private BulletPossessTarget possessedBullet;
    private BulletPossessTarget centreOfScreenPossessable;
    private PlayerPestilenceAbility pestilenceAbility;
    private bool bIsFocusing;
    private bool bLockOnStarted = false;
    private bool bLockedOn = false;
    private bool bCharged = false;
    private bool bPestilenceLockOn = false;
    private float lockOnTimer = 0f;
    private float lockOnTime = 1f;
    private float chargeTime = 1.5f;

    [SerializeField]
    private BulletPossessTarget freeCamBullet;

    public static EventHandler<BulletPossessTarget> OnNewCentralPossessable;
    public static EventHandler<BulletPossessTarget> OnNewBulletPossessed;

    public static EventHandler<bool> OnBulletCharging;

    private void OnEnable()
    {
        BulletPossessTarget.OnEmergencyRepossess += EmergencyPossess;
        pestilenceAbility = GetComponent<PlayerPestilenceAbility>();
    }

    private void OnDisable()
    {
        BulletPossessTarget.OnEmergencyRepossess -= EmergencyPossess;
    }

    private void Update()
    {
        if (!possessedBullet)
        {
            return;
        }

        FindCentreOfScreenPossessable();

        if (bLockOnStarted || bPestilenceLockOn)
        {
            lockOnTimer += Time.unscaledDeltaTime;

            if (lockOnTimer > lockOnTime)
            {
                bLockOnStarted = false;
                bLockedOn = true;
            }

            if (lockOnTimer > chargeTime)
            {
                bPestilenceLockOn = false;
                bCharged = true;
            }
        }
    }

    private void FindCentreOfScreenPossessable()
    {
        List<BulletPossessTarget> targets = possessedBullet.GetPossessables();

        if (targets.Count == 0)
        {
            if (centreOfScreenPossessable != null)
            {
                centreOfScreenPossessable = null;
                OnNewCentralPossessable?.Invoke(this, null);
            }
            return;
        }

        BulletPossessTarget closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (BulletPossessTarget target in targets)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(target.transform.position);

            // if (!target.GetBulletRenderer().isVisible)
            // {
            //     continue;
            // }

            if (
                viewPos.x < 0f
                || viewPos.x > 1f
                || viewPos.y < 0f
                || viewPos.y > 1f
                || viewPos.z < 0f
            )
            {
                continue;
            }

            //Debug.Log("Bullet " + target.name + " is currently " + viewPos.z + " z coords");

            Vector2 toCenter = viewPos - new Vector3(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null)
        {
            if (centreOfScreenPossessable != null)
            {
                centreOfScreenPossessable = null;
                OnNewCentralPossessable?.Invoke(this, null);
            }
            return;
        }

        if (closestTarget != centreOfScreenPossessable)
        {
            centreOfScreenPossessable = closestTarget;
            OnNewCentralPossessable?.Invoke(this, closestTarget);
        }
    }

    private void EmergencyPossess(object sender, BulletPossessTarget possessTarget)
    {
        if (possessTarget == null)
        {
            PossessFreeCamBullet();
        }
        else
        {
            PossessBullet(possessTarget);
        }
    }

    public void PossessFreeCamBullet()
    {
        if (!possessedBullet)
        {
            return;
        }

        if (freeCamBullet.transform.parent != null)
        {
            freeCamBullet.transform.parent = null;
        }

        //freeCamBullet.transform.position = possessedBullet.GetCameraTransform().position;
        freeCamBullet.transform.position = possessedBullet.transform.position;
        //freeCamBullet.transform.rotation = possessedBullet.GetCameraTransform().rotation;

        PossessBullet(freeCamBullet);
        freeCamBullet.GetComponent<BulletFreeCamMovement>().ToggleCamMovement(true);
    }

    public bool TryPossess()
    {
        if (!possessedBullet)
        {
            return false;
        }

        if (!centreOfScreenPossessable)
        {
            return false;
        }

        PossessBullet(centreOfScreenPossessable);
        return true;
    }

    public void PossessBullet(BulletPossessTarget newBullet)
    {
        Vector2 newBulletCameraAxis = new Vector2(0f, 0.5f);

        if (possessedBullet)
        {
            freeCamBullet.GetComponent<BulletFreeCamMovement>().ToggleCamMovement(false);

            newBulletCameraAxis = possessedBullet.GetCameraAxisValues();
            possessedBullet.UnpossessBullet();
        }
        //BulletPossess.BulletPossessed(this, possessedBullet);
        newBullet.PossessBullet(bIsFocusing, newBulletCameraAxis);
        possessedBullet = newBullet;
        OnNewBulletPossessed?.Invoke(this, possessedBullet);
    }

    public void UndoPossess(BulletPossessTarget newBullet)
    {
        Vector2 newBulletCameraAxis = new Vector2(0f, 0.5f);

        if (possessedBullet)
        {
            freeCamBullet.GetComponent<BulletFreeCamMovement>().ToggleCamMovement(false);

            newBulletCameraAxis = possessedBullet.GetCameraAxisValues();
            possessedBullet.UnpossessBullet();
        }

        if (!newBullet)
        {
            OnNewCentralPossessable?.Invoke(this, null);
            possessedBullet = null;
            return;
        }

        newBullet.PossessBullet(bIsFocusing, newBulletCameraAxis);
        possessedBullet = newBullet;
    }

    public void LockOnBullet()
    {
        if (!possessedBullet)
        {
            return;
        }

        bLockOnStarted = false;
        lockOnTimer = 0f;
        bPestilenceLockOn = false;
        bCharged = false;
        bLockedOn = false;

        bool lockOnTargetFound = possessedBullet.ToggleLockOn(true);

        if (!lockOnTargetFound)
        {
            return;
        }

        bLockOnStarted = true;

        if (
            pestilenceAbility
            && possessedBullet.TryGetComponent<BulletBooster>(out BulletBooster booster)
            && !booster.HasBoosted()
            && RedirectManager.Instance.CanBoost()
        )
        {
            bPestilenceLockOn = true;
            OnBulletCharging?.Invoke(this, true);
        }
    }

    public void RedirectBullet()
    {
        if (!possessedBullet)
        {
            return;
        }

        bLockOnStarted = false;
        bPestilenceLockOn = false;
        OnBulletCharging?.Invoke(this, false);

        if (bCharged)
        {
            bCharged = false;
            bLockedOn = false;
            possessedBullet.LockOnBullet();
            possessedBullet.GetComponent<BulletBooster>().CrystalBoost();
        }
        else if (bLockedOn)
        {
            bLockedOn = false;
            possessedBullet.LockOnBullet();
        }
        else
        {
            possessedBullet.RedirectBullet();
            possessedBullet.ToggleLockOn(false);
        }
    }

    public void SetIsFocusing(bool isFocusing)
    {
        bIsFocusing = isFocusing;
        if (!possessedBullet)
        {
            return;
        }
        possessedBullet.SetIsFocusing(isFocusing);
    }

    public BulletPossessTarget GetPossessedBullet()
    {
        return possessedBullet;
    }

    public BulletPossessTarget GetCentreOfScreenPossessable()
    {
        return centreOfScreenPossessable;
    }

    public bool CheckPossessedBullet(BulletPossessTarget possessTarget)
    {
        return possessedBullet == possessTarget;
    }
}
