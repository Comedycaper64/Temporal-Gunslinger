using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMaskShootState : State
{
    private float animationTimeNormalised;
    private readonly int ActiveAnimHash;
    private EnemyRangedStateMachine enemyStateMachine;
    private RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;

    public EnemyBossMaskShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyRangedStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        timer = enemyStateMachine.GetStateTimerSave();
        projectileFired = false;
        rewindState.ToggleMovement(true);

        stateMachine.stateMachineAnimator.CrossFade(
            ActiveAnimHash,
            0.01f,
            0,
            stateMachine.GetActiveAnimationExitTime()
        );

        if (timer >= shootTime)
        {
            projectileFired = true;
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;

            enemyStateMachine.SetProjectileAtFirePoint();
            enemyStateMachine.GetBulletStateMachine().SwitchToActive();
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }

    public override void Exit()
    {
        enemyStateMachine.SetStateTimerSave(timer);
        stateMachine.SetRunAnimationExitTime(animationTimeNormalised);
        rewindState.ToggleMovement(false);
    }
}
