using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    Animator animator;

    public EnemyIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("activate");
        }
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
