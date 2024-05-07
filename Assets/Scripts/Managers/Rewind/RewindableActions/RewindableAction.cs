using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewindableAction
{
    double timestamp;
    public static event EventHandler<RewindableAction> OnRewindableActionCreated;

    public virtual void Execute()
    {
        //RewindManager.Instance.AddRewindable(this);
        OnRewindableActionCreated?.Invoke(this, this);
    }

    public abstract void Undo();

    public virtual double GetTimestamp()
    {
        return timestamp;
    }

    public virtual void SetTimestamp(double timestamp)
    {
        this.timestamp = timestamp;
    }

    public virtual bool IsPriority()
    {
        return false;
    }
}
