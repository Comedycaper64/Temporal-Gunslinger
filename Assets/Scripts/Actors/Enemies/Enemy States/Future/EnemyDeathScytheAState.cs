using System;
using UnityEngine;

public class EnemyDeathScytheAState : State
{
    private float timer = 0f;
    private float animationTimeNormalised = 0f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;
    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Scythe Sweep 1";

    public EnemyDeathScytheAState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Scythe A Pattern ");

        deathSM.transform.position = deathSM.GetScytheAPosition().position;
        deathSM.transform.rotation = deathSM.GetScytheAPosition().rotation;

        deathSM.GetWeapon().ToggleScythe(true);
        deathSM.GetWeapon().GetScytheWeakPoint().OnHit += CounterAttack;

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

    public override void Exit()
    {
        deathSM.GetWeapon().ToggleScythe(false);
        deathSM.GetWeapon().GetScytheWeakPoint().OnHit -= CounterAttack;

        if (!rewindState.IsRewinding())
        {
            deathSM.AddAnimationTime(animationTimeNormalised);
            deathSM.AddDurationTime(timer);
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;

        // if (timer > attackTime)
        // {
        //     deathSM.SwitchState(
        //         new EnemyDeathTeleportBufferState(
        //             deathSM,
        //             new EnemyDeathRestingState(deathSM, true)
        //         )
        //     );
        // }
    }

    private void CounterAttack(object sender, EventArgs e)
    {
        deathSM.SwitchState(
            new EnemyDeathTeleportBufferState(deathSM, new EnemyDeathRestingState(deathSM, true))
        );
    }
}
