using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskStateMachine : EnemyRangedStateMachine
{
    // [SerializeField]
    // private bool fireProjectile;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.active, new BlankState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));

        //Debug.Log(stateDictionary.Count);
    }

    public void EnableFireProjectile()
    {
        //fireProjectile = true;
        stateDictionary.Remove(StateEnum.active);
        stateDictionary.Add(StateEnum.active, new EnemyRangedShootState(this));
    }
}
