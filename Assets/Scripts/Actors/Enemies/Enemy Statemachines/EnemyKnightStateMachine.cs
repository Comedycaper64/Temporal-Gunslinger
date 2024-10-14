using System.Collections.Generic;
using UnityEngine;

public class EnemyKnightStateMachine : StateMachine
{
    private Transform enemyStartPosition;

    [SerializeField]
    private float shootTimer;

    [SerializeField]
    private float throwAnimTimer;

    [SerializeField]
    private Transform projectileRestPoint;

    [SerializeField]
    private Transform projectileFirePoint;

    [SerializeField]
    private BulletStateMachine projectile;

    [SerializeField]
    private WeakPoint[] weakPoints;

    [SerializeField]
    private List<GameObject> bodyColliders = new List<GameObject>();

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyKnightIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyKnightThrowState(this));
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

    public float GetThrowTimer()
    {
        return throwAnimTimer;
    }

    public BulletStateMachine GetBulletStateMachine()
    {
        return projectile;
    }

    public void ResetPosition()
    {
        transform.position = enemyStartPosition.position;
    }

    public bool HasStartPosition()
    {
        return enemyStartPosition;
    }

    public void SetStartPosition(Transform newStart)
    {
        enemyStartPosition = newStart;
    }

    public void SetProjectileAtFirePoint()
    {
        Bullet bullet = projectile.GetComponent<Bullet>();
        bullet.SetFiringPosition(projectileFirePoint);
    }

    public void ResetProjectile()
    {
        Bullet bullet = projectile.GetComponent<Bullet>();
        bullet.ResetBullet(projectileRestPoint);
    }

    public void ToggleWeakPoints(bool toggle)
    {
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.gameObject.SetActive(toggle);
        }
    }
}
