using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : RewindableMovement
{
    [SerializeField]
    private bool moveToTarget = true;

    private Vector3 revenantOffset = new Vector3(0f, 1.4f, -0.25f);

    [SerializeField]
    private Transform damagePoint;

    private Transform movementTarget;

    private void Start()
    {
        movementTarget = GameManager.GetRevenant();
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

    protected override void StartMovement()
    {
        base.StartMovement();

        if (!moveToTarget)
        {
            return;
        }

        transform.LookAt(movementTarget.position + revenantOffset);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
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
                movementTarget.position + revenantOffset
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
