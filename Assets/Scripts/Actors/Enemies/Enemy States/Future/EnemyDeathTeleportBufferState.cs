using UnityEngine;

public class EnemyDeathTeleportBufferState : State
{
    private float timer = 0f;
    private float bufferTime = 0.005f;
    private State nextState;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    public EnemyDeathTeleportBufferState(StateMachine stateMachine, State nextState)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        this.nextState = nextState;
    }

    public override void Enter()
    {
        Debug.Log(" Death is Teleporting ");

        if (rewindState.IsRewinding())
        {
            timer = bufferTime;
        }
        else
        {
            timer = 0f;
            deathSM.GetDissolveController().StartDissolve(1f);
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        if (timer > bufferTime)
        {
            deathSM.SwitchState(nextState);
        }
    }

    public override void Exit()
    {
        if (!rewindState.IsRewinding())
        {
            deathSM.GetDissolveController().StartDissolve(deathSM.etherealDissolveValue);
        }
    }
}
