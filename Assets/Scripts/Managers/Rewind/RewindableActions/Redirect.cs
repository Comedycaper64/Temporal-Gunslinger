using UnityEngine;

public class Redirect : RewindableAction
{
    private Vector3 redirectPosition;
    private Vector3 initialDirection;
    private BulletMovement redirectedBullet;
    private float velocityAugment;
    private bool bIsRicochet;

    public static void BulletRedirected(
        Vector3 redirectPosition,
        Vector3 initialDirection,
        BulletMovement redirectedBullet,
        float velocityAugment,
        bool bIsRicochet
    )
    {
        new Redirect(
            redirectPosition,
            initialDirection,
            redirectedBullet,
            velocityAugment,
            bIsRicochet
        );
    }

    public Redirect(
        Vector3 redirectPosition,
        Vector3 initialDirection,
        BulletMovement redirectedBullet,
        float velocityAugment,
        bool bIsRicochet
    )
    {
        this.redirectPosition = redirectPosition;
        this.initialDirection = initialDirection;
        this.redirectedBullet = redirectedBullet;
        this.velocityAugment = velocityAugment;
        this.bIsRicochet = bIsRicochet;

        Execute();
    }

    public override void Undo()
    {
        redirectedBullet.UndoRedirect(
            redirectPosition,
            initialDirection,
            velocityAugment,
            bIsRicochet
        );
    }
}
