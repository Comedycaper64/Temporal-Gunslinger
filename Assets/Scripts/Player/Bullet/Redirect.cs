using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : IRewindable
{
    private float timestamp;
    private Vector3 redirectPosition;
    private Quaternion initialRotation;

    public Redirect(Vector3 redirectPosition, Quaternion initialRotation)
    {
        this.redirectPosition = redirectPosition;
        this.initialRotation = initialRotation;
    }

    public void Execute()
    {
        RewindManager.Instance.AddRewindable(this);
    }

    public float GetTimestamp()
    {
        return timestamp;
    }

    public void SetTimestamp(float timestamp)
    {
        this.timestamp = timestamp;
    }

    public void Undo()
    {
        //set position of bullet
        //set rotation of bullet
        //add redirect to redirect manager
    }
}
