using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeStateMachine : StateMachine
{
    // [SerializeField]
    // private VFXPlayback deathVFX;
    // public Animator animator;

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyMeleeActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public void Die()
    {
        //deathVFX.PlayEffect();
    }
}
