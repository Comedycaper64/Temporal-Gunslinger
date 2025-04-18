using System;
using UnityEngine;

public class EnemyDeadState : State
{
    public static int enemiesAlive;
    protected Animator animator;
    protected DissolveController dissolveController;

    public static EventHandler<int> OnEnemyDeadACH;

    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemiesAlive++;
        //Debug.Log("Alive enemies:" + enemiesAlive);
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

        //Debug.Log("Alive enemies:" + enemiesAlive);

        OnEnemyDeadACH?.Invoke(this, enemiesAlive);

        if (enemiesAlive <= 0)
        {
            GameManager.Instance.EndLevel(stateMachine.transform);
        }
    }

    public override void Exit()
    {
        //Debug.Log("Alive enemies: " + enemiesAlive);

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
