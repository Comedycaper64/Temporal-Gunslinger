using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewindableMovement : MonoBehaviour
{
    public static HashSet<RewindableMovement> Instances = new HashSet<RewindableMovement>();
    public abstract void BeginRewind();
    public abstract void BeginPlay();

    protected virtual void Awake()
    {
        Instances.Add(this);
    }

    protected virtual void OnDisable()
    {
        Instances.Remove(this);
    }
}
