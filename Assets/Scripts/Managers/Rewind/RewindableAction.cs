using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewindableAction
{
    float timestamp;
    public static event EventHandler<RewindableAction> OnRewindableActionCreated;

    public virtual void Execute()
    {
        //RewindManager.Instance.AddRewindable(this);
        OnRewindableActionCreated?.Invoke(this, this);
    }

    public abstract void Undo();

    public virtual float GetTimestamp()
    {
        return timestamp;
    }

    public virtual void SetTimestamp(float timestamp)
    {
        this.timestamp = timestamp;
    }
}
