using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeIdleState : State
{
    Animator animator;
    private EnemyMeleeStateMachine enemyMeleeStateMachine;

    public EnemyMeleeIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
        enemyMeleeStateMachine = stateMachine as EnemyMeleeStateMachine;
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("activate");
        }

        enemyMeleeStateMachine.ResetPosition();
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
