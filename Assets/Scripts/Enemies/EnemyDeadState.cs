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
        //Temp debug death
        //stateMachine.transform.position += new Vector3(0, -10, 0);
        if (animator)
        {
            animator.SetTrigger("death");
        }

        //enemyStateMachine.Die();
        enemyNumber--;
        if (enemyNumber <= 0)
        {
            GameManager.Instance.EndLevel();
        }
    }

    public override void Exit()
    {
        //stateMachine.transform.position += new Vector3(0, 10, 0);
        enemyNumber++;
    }

    public override void Tick(float deltaTime) { }
}
