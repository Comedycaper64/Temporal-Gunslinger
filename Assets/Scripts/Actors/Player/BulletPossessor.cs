using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessor : MonoBehaviour
{
    private BulletPossessTarget possessedBullet;
    private BulletPossessTarget centreOfScreenPossessable;
    private bool bIsFocusing;
    private bool bLockOnStarted = false;
    private bool bLockedOn = false;
    private float lockOnTimer = 0f;
    private float lockOnTime = 1f;

    public static EventHandler<BulletPossessTarget> OnNewCentralPossessable;
    public static EventHandler<BulletPossessTarget> OnNewBulletPossessed;

    private void Awake()
    {
        BulletPossessTarget.OnEmergencyRepossess += EmergencyPossess;
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

        if (bLockOnStarted)
        {
            lockOnTimer += Time.unscaledDeltaTime;

            if (lockOnTimer > lockOnTime)
            {
                bLockOnStarted = false;
                bLockedOn = true;
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
        PossessBullet(possessTarget);
    }

    public void TryPossess()
    {
        if (!possessedBullet)
        {
            return;
        }

        if (!centreOfScreenPossessable)
        {
            return;
        }

        PossessBullet(centreOfScreenPossessable);
    }

    // public void TryPossessNext()
    // {
    //     if (!possessedBullet)
    //     {
    //         return;
    //     }

    //     List<BulletPossessTarget> possessables = possessedBullet.GetPossessables();

    //     int currentPossessIndex = possessables.IndexOf(possessedBullet);
    //     int nextIndex = currentPossessIndex + 1;
    //     if (nextIndex >= possessables.Count)
    //     {
    //         nextIndex = 0;
    //     }

    //     PossessBullet(possessables[nextIndex]);
    // }

    // public void TryPossessPrevious()
    // {
    //     if (!possessedBullet)
    //     {
    //         return;
    //     }

    //     List<BulletPossessTarget> possessables = possessedBullet.GetPossessables();

    //     int currentPossessIndex = possessables.IndexOf(possessedBullet);
    //     int nextIndex = currentPossessIndex - 1;
    //     if (nextIndex < 0)
    //     {
    //         nextIndex = possessables.Count - 1;
    //     }

    //     PossessBullet(possessables[nextIndex]);
    // }

    public void PossessBullet(BulletPossessTarget newBullet)
    {
        Vector2 newBulletCameraAxis = new Vector2(0f, 0.5f);

        if (possessedBullet)
        {
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
        bLockOnStarted = true;
        lockOnTimer = 0f;

        possessedBullet.ToggleLockOn(true);
    }

    public void RedirectBullet()
    {
        if (!possessedBullet)
        {
            return;
        }

        bLockOnStarted = false;

        if (bLockedOn)
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

    public bool CheckPossessedBullet(BulletPossessTarget possessTarget)
    {
        return possessedBullet == possessTarget;
    }
}
