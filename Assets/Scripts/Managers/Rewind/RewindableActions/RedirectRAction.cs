using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectRAction : RewindableAction
{
    private BulletMovement redirectedBullet;

    public static void BulletRedirected(BulletMovement redirectedBullet)
    {
        RedirectRAction redirect = new RedirectRAction(redirectedBullet);
    }

    public RedirectRAction(BulletMovement redirectedBullet)
    {
        this.redirectedBullet = redirectedBullet;
        Execute();
    }

    public override void Undo()
    {
        redirectedBullet.UndoRedirect();
    }
}
