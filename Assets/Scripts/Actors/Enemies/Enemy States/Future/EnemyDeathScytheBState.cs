using UnityEngine;

public class EnemyDeathScytheBState : State
{
    private float timer = 0f;
    private float animationTimeNormalised = 0f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;
    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Scythe Sweep 2";

    public EnemyDeathScytheBState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Scythe B Pattern ");

        deathSM.transform.position = deathSM.GetScytheBPosition().position;
        deathSM.transform.rotation = deathSM.GetScytheBPosition().rotation;

        if (rewindState.IsRewinding())
        {
            timer = deathSM.GetDurationTime();
            deathSM.GetWeapon().SetTimeOffset(-timer);
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
            deathSM.GetWeapon().SetTimeOffset(0f);
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
        }
        deathSM.GetWeapon().ToggleScythe(true);
        deathSM.GetWeapon().OnHit += CounterAttack;
    }

    public override void Exit()
    {
        deathSM.GetWeapon().ToggleScythe(false);
        deathSM.GetWeapon().OnHit -= CounterAttack;

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

    private void CounterAttack()
    {
        deathSM.SwitchState(
            new EnemyDeathTeleportBufferState(deathSM, new EnemyDeathRestingState(deathSM, true))
        );
    }
}
