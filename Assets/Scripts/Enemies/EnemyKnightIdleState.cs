using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnightIdleState : State
{
    Animator animator;
    private EnemyKnightStateMachine enemyKnightStateMachine;

    public EnemyKnightIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
        enemyKnightStateMachine = stateMachine as EnemyKnightStateMachine;
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("activate");
        }

        enemyKnightStateMachine.ResetPosition();
        enemyKnightStateMachine.ResetProjectile();
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
