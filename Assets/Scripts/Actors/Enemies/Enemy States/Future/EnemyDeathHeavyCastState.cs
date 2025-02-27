using System;
using UnityEngine;

public class EnemyDeathHeavyCastState : State
{
    private float timer = 0f;
    private float animationTimeNormalised = 0f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Heavy Cast";

    public EnemyDeathHeavyCastState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Heavy Pattern ");

        deathSM.GetDissolveController().StartDissolve(0f);
        deathSM.ToggleShield(true);
        deathSM.ToggleWeakPoint(true);

        deathSM.GetHealth().OnDamageTaken += InterruptCast;

        if (rewindState.IsRewinding())
        {
            timer = deathSM.GetDurationTime();
            stateMachine.stateMachineAnimator.CrossFade(
                StateAnimHash,
                0f,
                0,
                deathSM.GetAnimationTime()
            );
        }
        else
        {
            timer = 0f;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;
    }

    public override void Exit()
    {
        deathSM.ToggleShield(false);
        deathSM.ToggleWeakPoint(false);

        deathSM.GetHealth().OnDamageTaken -= InterruptCast;

        if (!rewindState.IsRewinding())
        {
            deathSM.AddAnimationTime(animationTimeNormalised);
            deathSM.AddDurationTime(timer);
        }
    }

    private void InterruptCast()
    {
        deathSM.SwitchState(new EnemyDeathHeavyCastInterruptState(deathSM));
    }
}
