using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActiveState : State
{
    EnemyStateMachine enemyStateMachine;
    private float timer;
    private bool projectileFired;

    public EnemyActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyStateMachine;
    }

    public override void Enter()
    {
        timer = enemyStateMachine.GetShootTimer();
        projectileFired = false;
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime;
    }
}
