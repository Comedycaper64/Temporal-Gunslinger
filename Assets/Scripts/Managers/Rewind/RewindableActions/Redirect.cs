using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect //: RewindableAction
{
    Vector3 redirectPosition;
    Vector3 initialDirection;
    float velocityAugment;
    float timestamp;
    bool bIsRicochet;

    public Redirect(
        Vector3 redirectPosition,
        Vector3 initialDirection,
        float velocityAugment,
        float timestamp,
        bool bIsRicochet
    )
    {
        this.redirectPosition = redirectPosition;
        this.initialDirection = initialDirection;
        this.velocityAugment = velocityAugment;
        this.timestamp = timestamp;
        this.bIsRicochet = bIsRicochet;
    }

    public Vector3 GetRedirectPosition()
    {
        return redirectPosition;
    }

    public Vector3 GetInitialDirection()
    {
        return initialDirection;
    }

    public float GetVelocityAugment()
    {
        return velocityAugment;
    }

    public float GetTimestamp()
    {
        return timestamp;
    }

    public bool IsRicochet()
    {
        return bIsRicochet;
    }

    // private Vector3 redirectPosition;
    // private Vector3 initialDirection;
    // private BulletMovement redirectedBullet;
    // private float velocityAugment;
    // private bool bIsRicochet;

    // public static void BulletRedirected(
    //     Vector3 redirectPosition,
    //     Vector3 initialDirection,
    //     BulletMovement redirectedBullet,
    //     float velocityAugment,
    //     bool bIsRicochet
    // )
    // {
    //     Redirect newRedirect = new Redirect(
    //         redirectPosition,
    //         initialDirection,
    //         redirectedBullet,
    //         velocityAugment,
    //         bIsRicochet
    //     );
    // }

    // public Redirect(
    //     Vector3 redirectPosition,
    //     Vector3 initialDirection,
    //     BulletMovement redirectedBullet,
    //     float velocityAugment,
    //     bool bIsRicochet
    // )
    // {
    //     this.redirectPosition = redirectPosition;
    //     this.initialDirection = initialDirection;
    //     this.redirectedBullet = redirectedBullet;
    //     this.velocityAugment = velocityAugment;
    //     this.bIsRicochet = bIsRicochet;

    //     Execute();
    // }

    // public override void Undo()
    // {
    //     redirectedBullet.UndoRedirect(
    //         redirectPosition,
    //         initialDirection,
    //         velocityAugment,
    //         bIsRicochet
    //     );
    // }
}
