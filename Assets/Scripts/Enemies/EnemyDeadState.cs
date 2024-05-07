using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : State
{
    public static int enemyNumber;
    private Animator animator;
    private DissolveController dissolveController;

    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyNumber++;
        animator = stateMachine.stateMachineAnimator;
        dissolveController = stateMachine.dissolveController;
    }

    public override void Enter()
    {
        // if (animator)
        // {
        //     animator.SetTrigger("death");
        // }

        if (dissolveController)
        {
            dissolveController.StartDissolve();
        }

        stateMachine.ToggleInactive(true);
        enemyNumber--;
        //Debug.Log("Enemies remaining: " + enemyNumber);
        if (enemyNumber <= 0)
        {
            GameManager.Instance.EndLevel(stateMachine.transform);
        }
    }

    public override void Exit()
    {
        if (dissolveController)
        {
            dissolveController.StopDissolve();
        }

        stateMachine.ToggleInactive(false);
        enemyNumber++;
    }

    public override void Tick(float deltaTime) { }
}
