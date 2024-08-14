using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessor : MonoBehaviour
{
    private BulletPossessTarget possessedBullet;
    private BulletPossessTarget centreOfScreenPossessable;
    private bool bIsFocusing;

    public static EventHandler<BulletPossessTarget> OnNewCentralPossessable;

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
            Vector2 viewPos = Camera.main.WorldToViewportPoint(target.transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
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
        if (possessedBullet)
        {
            possessedBullet.UnpossessBullet();
        }
        //BulletPossess.BulletPossessed(this, possessedBullet);
        newBullet.PossessBullet(bIsFocusing);
        possessedBullet = newBullet;
    }

    public void UndoPossess(BulletPossessTarget newBullet)
    {
        if (possessedBullet)
        {
            possessedBullet.UnpossessBullet();
        }

        if (!newBullet)
        {
            OnNewCentralPossessable?.Invoke(this, null);
            possessedBullet = null;
            return;
        }

        newBullet.PossessBullet(bIsFocusing);
        possessedBullet = newBullet;
    }

    public void RedirectBullet()
    {
        if (!possessedBullet)
        {
            return;
        }

        possessedBullet.RedirectBullet();
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
