using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeStateMachine : StateMachine
{
    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.active, new EnemyMeleeActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }
}
