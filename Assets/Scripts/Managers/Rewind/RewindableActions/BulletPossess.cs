public class BulletPossess : RewindableAction
{
    private BulletPossessor possessor;
    private BulletPossessTarget previousTarget;

    public static void BulletPossessed(
        BulletPossessor bulletPossessor,
        BulletPossessTarget previousTarget
    )
    {
        new BulletPossess(bulletPossessor, previousTarget);
    }

    public BulletPossess(BulletPossessor bulletPossessor, BulletPossessTarget previousTarget)
    {
        this.previousTarget = previousTarget;
        possessor = bulletPossessor;
        Execute();
    }

    public override void Undo()
    {
        possessor.UndoPossess(previousTarget);
    }
}
