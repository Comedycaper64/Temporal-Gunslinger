using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretStateMachine : EnemyRangedStateMachine
{
    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.active, new EnemyTurretActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }
}
