using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedShootState : State
{
    EnemyRangedStateMachine enemyStateMachine;

    public EnemyRangedShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyRangedStateMachine;
    }

    public override void Enter()
    {
        enemyStateMachine.SetProjectileAtFirePoint();
        enemyStateMachine.GetBulletStateMachine().SwitchToActive();
        stateMachine.stateMachineAnimator.SetBool("shot", true);
    }

    public override void Exit()
    {
        stateMachine.stateMachineAnimator.SetBool("shot", false);
    }

    public override void Tick(float deltaTime) { }
}
