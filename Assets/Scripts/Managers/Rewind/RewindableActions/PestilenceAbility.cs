public class PestilenceAbility : RewindableAction
{
    private float initialSpeed;
    private float initialLoss;
    private BulletBooster boostedBullet;

    public static void BulletBoosted(BulletBooster bullet, float initialSpeed, float initialLoss)
    {
        new PestilenceAbility(bullet, initialSpeed, initialLoss);
    }

    public PestilenceAbility(BulletBooster bullet, float initialSpeed, float initialLoss)
    {
        boostedBullet = bullet;
        this.initialSpeed = initialSpeed;
        this.initialLoss = initialLoss;

        Execute();
    }

    public override void Undo()
    {
        boostedBullet.UndoBoost(initialSpeed, initialLoss);
    }
}
