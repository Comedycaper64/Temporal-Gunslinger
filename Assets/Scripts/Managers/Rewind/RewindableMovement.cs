using System.Collections.Generic;
using UnityEngine;

public abstract class RewindableMovement : MonoBehaviour
{
    protected bool movementActive = false;

    [SerializeField]
    protected float startSpeed = 1f;
    private float speed = 0f;
    private static float timeScale = 1f;

    private const float timeScaleLowerLimit = 0.002f;
    public static HashSet<RewindableMovement> Instances = new HashSet<RewindableMovement>();

    public virtual void BeginRewind()
    {
        speed = Mathf.Abs(speed) * -1;
        startSpeed = Mathf.Abs(startSpeed) * -1;
    }

    public virtual void BeginPlay()
    {
        speed = Mathf.Abs(speed);
        startSpeed = Mathf.Abs(startSpeed);
    }

    protected virtual void StartMovement()
    {
        speed = startSpeed;
        movementActive = true;
    }

    protected virtual void StopMovement()
    {
        speed = 0f;
        movementActive = false;
    }

    public virtual void ToggleMovement(bool toggle)
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

    public float GetStartSpeed()
    {
        return startSpeed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    protected bool IsActive()
    {
        return movementActive;
    }

    protected virtual void OnEnable()
    {
        Instances.Add(this);
    }

    protected virtual void OnDisable()
    {
        Instances.Remove(this);
    }

    public static void UpdateMovementTimescale(float newTimeScale)
    {
        //timeScale = Mathf.Clamp01(newTimeScale);
        timeScale = Mathf.Clamp(newTimeScale, timeScaleLowerLimit, 1f);
    }

    public static float GetTimescale()
    {
        return timeScale;
    }
}
