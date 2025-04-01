using UnityEngine;

public class EnemyDeathRestingState : State
{
    private bool teleport;
    private float timer = 0f;
    private float restTime = 0.01f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Active Idle";

    public EnemyDeathRestingState(StateMachine stateMachine, bool teleport)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        this.teleport = teleport;
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0.01f, 0);

        //If teleport and not reversing, start off dissolves, teleport to rest point, then undissolve
        // if (teleport)
        // {
        deathSM.transform.position = deathSM.GetRestPosition().position;
        deathSM.transform.rotation = deathSM.GetRestPosition().rotation;
        //}

        if (rewindState.IsRewinding())
        {
            timer = restTime;
            deathSM.DecrementFlow();
        }
        else
        {
            timer = 0f;
        }
    }

    public override void Tick(float deltaTime)
    {
        //Count to rest time, then get next state from SM and switch to it
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        if (timer > restTime)
        {
            deathSM.SwitchState(new EnemyDeathTeleportBufferState(deathSM, deathSM.GetNextState()));
        }
    }

    public override void Exit()
    {
        if (!rewindState.IsRewinding())
        {
            deathSM.IncrementFlow();
        }
    }
}
