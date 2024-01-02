using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : IRewindableAction
{
    private float timestamp;
    private Vector3 redirectPosition;
    private Quaternion initialRotation;
    private Bullet redirectedBullet;

    public static void BulletRedirected(
        Vector3 redirectPosition,
        Quaternion initialRotation,
        Bullet redirectedBullet
    )
    {
        if (!RewindManager.Instance)
        {
            return;
        }

        Redirect newRedirect = new Redirect(redirectPosition, initialRotation, redirectedBullet);
    }

    public Redirect(Vector3 redirectPosition, Quaternion initialRotation, Bullet redirectedBullet)
    {
        this.redirectPosition = redirectPosition;
        this.initialRotation = initialRotation;
        this.redirectedBullet = redirectedBullet;

        Execute();
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
        redirectedBullet.UndoRedirect(redirectPosition, initialRotation);
    }
}
