using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedStateMachine : StateMachine
{
    [SerializeField]
    protected float shootTimer;

    [SerializeField]
    protected string aimingAnimationName;

    [SerializeField]
    protected Transform projectileRestPoint;

    [SerializeField]
    protected Transform projectileFirePoint;

    [SerializeField]
    protected BulletStateMachine projectile;

    [SerializeField]
    protected List<GameObject> bodyColliders = new List<GameObject>();

    [SerializeField]
    protected VFXPlayback shootVFX;

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyRangedActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        foreach (GameObject collider in bodyColliders)
        {
            collider.SetActive(!toggle);
        }
    }

    public float GetShootTimer()
    {
        return shootTimer;
    }

    public string GetAimingAnimationName()
    {
        return aimingAnimationName;
    }

    public BulletStateMachine GetBulletStateMachine()
    {
        return projectile;
    }

    public virtual void SetProjectileAtFirePoint()
    {
        Bullet bullet = projectile.GetComponent<Bullet>();
        bullet.SetFiringPosition(projectileFirePoint);
    }

    public virtual void ResetProjectile()
    {
        Bullet bullet = projectile.GetComponent<Bullet>();
        bullet.ResetBullet(projectileRestPoint);
        SetRunAnimationExitTime(0f);
        //Debug.Log("Animation Time Reset");
    }

    public virtual void PlayShootFX()
    {
        if (shootVFX)
        {
            shootVFX.PlayEffect();
        }
    }

    // public override void SwitchState(State newState)
    // {
    //     base.SwitchState(newState);
    //     Debug.Log("Current State: " + newState.GetStateName());
    // }
}