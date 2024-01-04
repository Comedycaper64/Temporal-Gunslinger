using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : RewindableAction
{
    private Vector3 redirectPosition;
    private Quaternion initialRotation;
    private Bullet redirectedBullet;

    public static void BulletRedirected(
        Vector3 redirectPosition,
        Quaternion initialRotation,
        Bullet redirectedBullet
    )
    {
        Redirect newRedirect = new Redirect(redirectPosition, initialRotation, redirectedBullet);
    }

    public Redirect(Vector3 redirectPosition, Quaternion initialRotation, Bullet redirectedBullet)
    {
        this.redirectPosition = redirectPosition;
        this.initialRotation = initialRotation;
        this.redirectedBullet = redirectedBullet;

        Execute();
    }

    public override void Undo()
    {
        redirectedBullet.UndoRedirect(redirectPosition, initialRotation);
    }
}
