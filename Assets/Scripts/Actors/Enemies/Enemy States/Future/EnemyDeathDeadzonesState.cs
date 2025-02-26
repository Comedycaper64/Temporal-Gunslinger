using UnityEngine;

public class EnemyDeathDeadzonesState : State
{
    private bool deadzonesSpawned = false;
    private float timer = 0f;
    private float spawnTime = 0.02f;
    private float stateTime = 0.06f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Conjure Deadzones";

    public EnemyDeathDeadzonesState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Deadzone Pattern ");

        deathSM.GetDissolveController().StartDissolve(deathSM.etherealDissolveValue);

        if (rewindState.IsRewinding())
        {
            timer = stateTime;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0, 1f);
            deadzonesSpawned = true;
        }
        else
        {
            timer = 0f;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
            deadzonesSpawned = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        if (timer > stateTime)
        {
            deathSM.SwitchState(new EnemyDeathRestingState(deathSM, false));
        }
    }

    public override void Exit() { }
}
