using UnityEngine;

public class EnemyDeathScytheBState : State
{
    private float timer = 0f;
    private float attackTime = 0.075f;
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
            timer = attackTime;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0, 1f);
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

        if (timer > attackTime)
        {
            deathSM.SwitchState(
                new EnemyDeathTeleportBufferState(
                    deathSM,
                    new EnemyDeathRestingState(deathSM, true)
                )
            );
        }
    }

    public override void Exit() { }
}
