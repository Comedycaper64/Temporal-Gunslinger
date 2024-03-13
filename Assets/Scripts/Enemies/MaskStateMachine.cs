using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskStateMachine : EnemyRangedStateMachine
{
    [SerializeField]
    private bool fireProjectile;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyInactiveState(this));
        if (fireProjectile)
        {
            stateDictionary.Add(StateEnum.active, new EnemyRangedShootState(this));
        }
        else
        {
            stateDictionary.Add(StateEnum.active, new EnemyInactiveState(this));
        }

        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleInactive(bool toggle) { }
}
