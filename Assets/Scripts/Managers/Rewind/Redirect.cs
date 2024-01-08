using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : RewindableAction
{
    private Vector3 redirectPosition;
    private Vector3 initialDirection;
    private Bullet redirectedBullet;

    public static void BulletRedirected(
        Vector3 redirectPosition,
        Vector3 initialDirection,
        Bullet redirectedBullet
    )
    {
        Redirect newRedirect = new Redirect(redirectPosition, initialDirection, redirectedBullet);
    }

    public Redirect(Vector3 redirectPosition, Vector3 initialDirection, Bullet redirectedBullet)
    {
        this.redirectPosition = redirectPosition;
        this.initialDirection = initialDirection;
        this.redirectedBullet = redirectedBullet;

        Execute();
    }

    public override void Undo()
    {
        redirectedBullet.UndoRedirect(redirectPosition, initialDirection);
    }
}
