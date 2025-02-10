using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedStateMachine : StateMachine
{
    protected float stateTimerSave = 0f;

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

    public virtual void Start()
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

    public float GetStateTimerSave()
    {
        return stateTimerSave;
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

    public void SetStateTimerSave(float savedTimer)
    {
        stateTimerSave = savedTimer;
    }

    public virtual void ResetProjectile()
    {
        SetRunAnimationExitTime(0f);

        if (projectile)
        {
            Bullet bullet = projectile.GetComponent<Bullet>();
            bullet.ResetBullet(projectileRestPoint);
        }

        //Debug.Log("Animation Time Reset");
    }

    public virtual void PlayShootFX()
    {
        if (shootVFX)
        {
            shootVFX.PlayEffect();
        }
    }
}
