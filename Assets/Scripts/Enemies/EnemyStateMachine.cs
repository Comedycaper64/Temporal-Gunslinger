using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    // public GameObject projectile;
    // public Transform emitter;

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

    // public void EnterDeadState()
    // {
    //     SwitchState(new EnemyDeadState(this));
    // }
}
