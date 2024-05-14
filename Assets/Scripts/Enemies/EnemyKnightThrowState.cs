using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnightThrowState : State
{
    EnemyKnightStateMachine enemyStateMachine;
    RewindState rewindState;
    private float timer;
    private float shootTime;
    private float throwTime;
    private float animationSpeedMult = 100f;
    private bool projectileFired;
    private readonly int ThrowAnimHash = Animator.StringToHash("Knight Throw Charge");

    public EnemyKnightThrowState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyKnightStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        throwTime = enemyStateMachine.GetThrowTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
    }

    public override void Enter()
    {
        timer = 0f;
        projectileFired = false;
        //stateMachine.stateMachineAnimator.SetTrigger("throw");

        rewindState.ToggleMovement(true);
        float animationStartTime = 0f;
        if (rewindState.GetTimeSpeed() < 0f)
        {
            timer = shootTime;
            animationStartTime = 1f;
            projectileFired = true;
        }

        stateMachine.stateMachineAnimator.Play(ThrowAnimHash, 0, animationStartTime);
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed() * animationSpeedMult;

        //Debug.Log(timer);

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;
            enemyStateMachine.GetBulletStateMachine().SwitchToActive();
        }
        else if (projectileFired && timer >= throwTime)
        {
            enemyStateMachine.SwitchState(new EnemyMeleeActiveState(stateMachine));
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }
}
