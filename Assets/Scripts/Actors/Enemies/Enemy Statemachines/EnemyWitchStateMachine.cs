using UnityEngine;

public class EnemyWitchStateMachine : EnemyRangedStateMachine
{
    private bool[] bulletsFired = { false, false, false };
    private LockOnTarget revenantTarget;

    [SerializeField]
    protected Transform secondaryProjectileFirePoint;

    [SerializeField]
    protected BulletStateMachine secondaryProjectile;

    [SerializeField]
    protected float secondaryShootTimer;

    [SerializeField]
    protected Transform thirdProjectileFirePoint;

    [SerializeField]
    protected BulletStateMachine thirdProjectile;

    [SerializeField]
    protected float thirdShootTimer;

    public override void Start()
    {
        base.Start();

        revenantTarget = GameManager.GetRevenant().GetComponent<LockOnTarget>();
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyWitchActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    private BulletStateMachine[] GetBulletStateMachines()
    {
        return new[] { projectile, secondaryProjectile, thirdProjectile };
    }

    private Transform[] GetBulletFiringPositions()
    {
        return new[]
        {
            projectileFirePoint,
            secondaryProjectileFirePoint,
            thirdProjectileFirePoint
        };
    }

    public bool[] GetBulletsFired()
    {
        return bulletsFired;
    }

    public float[] GetShootTimes()
    {
        return new[] { shootTimer, secondaryShootTimer, thirdShootTimer };
    }

    public void FireBullet(int bulletIndex)
    {
        Bullet firingBullet = GetBulletStateMachines()[bulletIndex].GetComponent<Bullet>();
        firingBullet.SetFiringPosition(GetBulletFiringPositions()[bulletIndex]);

        GetBulletStateMachines()[bulletIndex].SwitchToActive();

        BulletLockOn bulletLockOn = firingBullet.GetComponent<BulletLockOn>();
        bulletLockOn.LockOnOverride(revenantTarget);

        GetBulletsFired()[bulletIndex] = true;
    }

    public void UnfireBullet(int bulletIndex)
    {
        GetBulletsFired()[bulletIndex] = false;
    }

    public override void ResetProjectile()
    {
        Bullet bullet1 = projectile.GetComponent<Bullet>();
        Bullet bullet2 = secondaryProjectile.GetComponent<Bullet>();
        Bullet bullet3 = thirdProjectile.GetComponent<Bullet>();
        bullet1.ResetBullet(projectileRestPoint);
        bullet2.ResetBullet(projectileRestPoint);
        bullet3.ResetBullet(projectileRestPoint);
        SetRunAnimationExitTime(0f);
        //Debug.Log("Animation Time Reset");
    }
}
