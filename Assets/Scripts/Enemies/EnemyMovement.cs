using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : RewindableMovement
{
    [SerializeField]
    private Transform damagePoint;

    private Transform movementTarget;

    private void Start()
    {
        movementTarget = GameManager.GetRevenant();
        DangerTracker.dangers.Add(this);
    }

    private void Update()
    {
        transform.position += transform.forward * GetSpeed() * Time.deltaTime;
    }

    public bool WillKillRevenant(out float deathTime)
    {
        if (Mathf.Abs(GetUnscaledSpeed()) > 0f)
        {
            float distanceToTarget = Vector3.Distance(
                damagePoint.position,
                movementTarget.position
            );
            float timeToTarget = distanceToTarget / Mathf.Abs(GetStartSpeed());
            deathTime = timeToTarget;
            return true;
        }
        else
        {
            deathTime = -1f;
            return false;
        }
    }
}
