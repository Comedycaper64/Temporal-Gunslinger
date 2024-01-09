using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessor : MonoBehaviour
{
    private BulletPossessTarget possessedBullet;
    private bool bIsFocusing;

    public void TryPossess()
    {
        if (!possessedBullet || !bIsFocusing)
        {
            return;
        }

        List<BulletPossessTarget> targets = possessedBullet.GetPossessables();

        if (targets.Count == 0)
        {
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
            return;
        }

        PossessBullet(closestTarget);
    }

    public void PossessBullet(BulletPossessTarget newBullet)
    {
        if (possessedBullet)
        {
            possessedBullet.UnpossessBullet();
        }
        BulletPossess.BulletPossessed(this, possessedBullet);
        newBullet.PossessBullet();
        possessedBullet = newBullet;
    }

    public void UndoPossess(BulletPossessTarget newBullet)
    {
        possessedBullet.UnpossessBullet();
        if (!newBullet)
        {
            possessedBullet = null;
            return;
        }

        newBullet.PossessBullet();
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
        if (!possessedBullet)
        {
            return;
        }
        bIsFocusing = isFocusing;
        possessedBullet.SetIsFocusing(isFocusing);
    }
}
