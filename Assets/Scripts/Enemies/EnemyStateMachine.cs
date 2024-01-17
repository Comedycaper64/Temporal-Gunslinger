using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField]
    private float shootTimer;

    [SerializeField]
    private BulletStateMachine projectile;

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.active, new EnemyActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public float GetShootTimer()
    {
        return shootTimer;
    }

    public BulletStateMachine GetBulletStateMachine()
    {
        return projectile;
    }
}
