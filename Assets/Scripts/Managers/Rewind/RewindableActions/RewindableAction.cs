using System;

public abstract class RewindableAction
{
    double timestamp;
    public static event EventHandler<RewindableAction> OnRewindableActionCreated;

    public virtual void Execute()
    {
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
