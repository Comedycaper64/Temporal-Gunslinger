using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewindableMovement : MonoBehaviour
{
    [SerializeField]
    protected float startSpeed = 1f;
    protected float speed = 0f;

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

    private void StartMovement()
    {
        speed = startSpeed;
    }

    private void StopMovement()
    {
        speed = 0f;
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

    protected virtual void Awake()
    {
        Instances.Add(this);
    }

    protected virtual void OnDisable()
    {
        Instances.Remove(this);
    }
}
