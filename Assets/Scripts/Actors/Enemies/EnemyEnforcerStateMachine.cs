using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnforcerStateMachine : EnemyRangedStateMachine
{
    [SerializeField]
    protected Transform secondaryProjectileRestPoint;

    [SerializeField]
    protected Transform secondaryProjectileFirePoint;

    [SerializeField]
    protected BulletStateMachine secondaryProjectile;

    [SerializeField]
    protected VFXPlayback secondaryGunfireVFX;

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyEnforcerActiveState(this)); // change to enforcer state
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public BulletStateMachine[] GetBulletStateMachines()
    {
        return new[] { projectile, secondaryProjectile };
    }

    public override void SetProjectileAtFirePoint()
    {
        Bullet bullet1 = projectile.GetComponent<Bullet>();
        Bullet bullet2 = secondaryProjectile.GetComponent<Bullet>();
        bullet1.SetFiringPosition(projectileFirePoint);
        bullet2.SetFiringPosition(secondaryProjectileFirePoint);
    }

    public override void ResetProjectile()
    {
        Bullet bullet1 = projectile.GetComponent<Bullet>();
        Bullet bullet2 = secondaryProjectile.GetComponent<Bullet>();
        bullet1.ResetBullet(projectileRestPoint);
        bullet2.ResetBullet(secondaryProjectileRestPoint);
        SetRunAnimationExitTime(0f);
        //Debug.Log("Animation Time Reset");
    }

    public override void PlayShootFX()
    {
        if (shootVFX)
        {
            shootVFX.PlayEffect();
            UnparentObject.ObjectUnparented(
                shootVFX.transform,
                shootVFX.transform.parent,
                shootVFX.transform.position
            );
        }

        if (secondaryGunfireVFX)
        {
            secondaryGunfireVFX.PlayEffect();
            UnparentObject.ObjectUnparented(
                secondaryGunfireVFX.transform,
                secondaryGunfireVFX.transform.parent,
                secondaryGunfireVFX.transform.position
            );
        }
    }
}
