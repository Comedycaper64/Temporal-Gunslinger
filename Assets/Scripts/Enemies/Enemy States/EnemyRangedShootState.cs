using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedShootState : State
{
    private float animationTimeNormalised;

    EnemyRangedStateMachine enemyStateMachine;
    private readonly int ActiveAnimHash;

    public EnemyRangedShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyRangedStateMachine;
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        stateMachine.stateMachineAnimator.SetBool("shot", true);

        //Debug.Log("Animation Time: " + stateMachine.GetActiveAnimationExitTime());

        stateMachine.stateMachineAnimator.CrossFade(
            ActiveAnimHash,
            0.02f,
            0,
            stateMachine.GetActiveAnimationExitTime()
        );

        //Debug.Log(stateMachine.GetActiveAnimationExitTime());

        if (stateMachine.GetActiveAnimationExitTime() < 0.01f)
        {
            enemyStateMachine.SetProjectileAtFirePoint();
            enemyStateMachine.GetBulletStateMachine().SwitchToActive();
        }
    }

    public override void Tick(float deltaTime)
    {
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.stateMachineAnimator.SetBool("shot", false);
        stateMachine.SetRunAnimationExitTime(animationTimeNormalised);
    }
}
