using UnityEngine;

public class EnemyDeathScytheAAltState : State
{
    private bool quillThrown = false;
    private float timer = 0f;
    private float throwTime = 0.01f;
    private float attackTime = 0.075f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;
    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Scythe Sweep 1 Alt";

    public EnemyDeathScytheAAltState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Scythe A Alt Pattern ");

        deathSM.transform.position = deathSM.GetScytheAPosition().position;
        deathSM.transform.rotation = deathSM.GetScytheAPosition().rotation;

        if (rewindState.IsRewinding())
        {
            timer = attackTime;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0, 1f);
            quillThrown = true;
        }
        else
        {
            timer = 0f;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
            quillThrown = false;
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
