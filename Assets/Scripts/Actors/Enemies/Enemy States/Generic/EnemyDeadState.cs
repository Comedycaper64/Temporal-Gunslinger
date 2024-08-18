using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : State
{
    public static int enemiesAlive;
    private Animator animator;
    private DissolveController dissolveController;

    //public static EventHandler<float> OnEnemyDeadChange;

    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemiesAlive++;
        animator = stateMachine.stateMachineAnimator;
        dissolveController = stateMachine.GetDissolveController();
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetBool("death", true);
        }

        if (dissolveController)
        {
            dissolveController.StartDissolve();
        }

        stateMachine.ToggleInactive(true);
        enemiesAlive--;

        //OnEnemyDeadChange?.Invoke();

        //Debug.Log("Enemies remaining: " + enemyNumber);
        if (enemiesAlive <= 0)
        {
            GameManager.Instance.EndLevel(stateMachine.transform);
        }
    }

    public override void Exit()
    {
        Debug.Log("Alive enemies: " + enemiesAlive);

        if (enemiesAlive > 0)
        {
            enemiesAlive++;

            //OnEnemyDeadChange?.Invoke();

            if (animator)
            {
                animator.SetBool("death", false);
            }
        }

        if (dissolveController && (enemiesAlive > 0))
        {
            dissolveController.StopDissolve();
        }

        stateMachine.ToggleInactive(false);
    }

    public override void Tick(float deltaTime) { }
}
