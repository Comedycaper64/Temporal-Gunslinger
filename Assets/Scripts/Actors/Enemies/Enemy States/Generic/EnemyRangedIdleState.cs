using UnityEngine;

public class EnemyRangedIdleState : State
{
    private Animator animator;
    private EnemyRangedStateMachine enemyRangedStateMachine;

    public EnemyRangedIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;

        //if (stateMachine.GetType() == typeof(EnemyRangedStateMachine))
        //{
        enemyRangedStateMachine = stateMachine as EnemyRangedStateMachine;
        //}
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("activate");
        }

        if (enemyRangedStateMachine)
        {
            enemyRangedStateMachine.ResetProjectile();
            enemyRangedStateMachine.SetStateTimerSave(0f);
        }
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
