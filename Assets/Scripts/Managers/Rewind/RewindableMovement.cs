using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewindableMovement : MonoBehaviour
{
    protected bool movementActive = false;

    [SerializeField]
    protected float startSpeed = 1f;
    private float speed = 0f;
    private static float timeScale = 1f;

    //public static HashSet<RewindableMovement> Instances = new HashSet<RewindableMovement>();

    // public virtual void BeginRewind()
    // {
    //     speed = Mathf.Abs(speed) * -1;
    //     startSpeed = Mathf.Abs(startSpeed) * -1;
    // }

    // public virtual void BeginPlay()
    // {
    //     speed = Mathf.Abs(speed);
    //     startSpeed = Mathf.Abs(startSpeed);
    // }

    private void StartMovement()
    {
        speed = startSpeed;
        movementActive = true;
    }

    private void StopMovement()
    {
        speed = 0f;
        movementActive = false;
    }

    public void ToggleMovement(bool toggle)
    {
        if (toggle)
        {
            StartMovement();
        }
        else
        {
            StopMovement();
        }
    }

    protected float GetSpeed()
    {
        // Debug.Log("Speed: " + speed + " Timescale: " + timeScale);
        return speed * timeScale;
    }

    protected float GetUnscaledSpeed()
    {
        return speed;
    }

    protected void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    protected bool IsActive()
    {
        return movementActive;
    }

    protected bool IsRewinding()
    {
        return RewindManager.bRewinding;
    }

    protected int GetRewindMultiplier()
    {
        if (IsRewinding())
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    protected float GetRewindTime()
    {
        return RewindManager.GetRewindTime();
    }

    // protected virtual void Awake()
    // {
    //     Instances.Add(this);
    // }

    // protected virtual void OnDisable()
    // {
    //     Instances.Remove(this);
    // }

    public static void UpdateMovementTimescale(float newTimeScale)
    {
        timeScale = Mathf.Clamp01(newTimeScale);
    }
}
