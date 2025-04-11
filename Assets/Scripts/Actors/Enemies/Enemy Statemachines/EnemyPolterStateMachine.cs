public class EnemyPolterStateMachine : EnemyRangedStateMachine
{
    private LockOnTarget revenantTarget;

    public override void Start()
    {
        base.Start();
        revenantTarget = GameManager.GetRevenant().GetComponent<LockOnTarget>();
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyPolterThrowState(this));
        stateDictionary.Add(StateEnum.dead, new BlankState(this));
    }

    public void LockOnBullet()
    {
        BulletLockOn bulletLockOn = projectile.GetComponent<BulletLockOn>();
        bulletLockOn.LockOnOverride(revenantTarget);
    }

    public void StopShootFX()
    {
        shootVFX.StopEffect();
    }

    public override void ResetProjectile()
    {
        base.ResetProjectile();
        shootVFX.StopEffect();
    }
}
