using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnforcerShootState : State
{
    private float animationTimeNormalised;

    private EnemyEnforcerStateMachine enemyStateMachine;
    private readonly int ActiveAnimHash;

    public EnemyEnforcerShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyEnforcerStateMachine;
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

        if (stateMachine.GetActiveAnimationExitTime() < 0.01f)
        {
            enemyStateMachine.SetProjectileAtFirePoint();

            BulletStateMachine[] bullets = enemyStateMachine.GetBulletStateMachines();

            foreach (BulletStateMachine bullet in bullets)
            {
                bullet.SwitchToActive();
            }
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
