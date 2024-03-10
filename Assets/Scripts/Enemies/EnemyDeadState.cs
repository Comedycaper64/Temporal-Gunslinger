using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : State
{
    public static int enemyNumber;
    Animator animator;

    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyNumber++;
        animator = stateMachine.stateMachineAnimator;
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("death");
        }

        stateMachine.ToggleDie(true);
        enemyNumber--;
        if (enemyNumber <= 0)
        {
            GameManager.Instance.EndLevel(stateMachine.transform);
        }
    }

    public override void Exit()
    {
        stateMachine.ToggleDie(false);
        enemyNumber++;
    }

    public override void Tick(float deltaTime) { }
}
