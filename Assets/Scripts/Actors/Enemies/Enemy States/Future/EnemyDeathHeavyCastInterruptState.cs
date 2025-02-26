using UnityEngine;

public class EnemyDeathHeavyCastInterruptState : State
{
    private float timer = 0f;
    private float stateTime = 0.02f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Heavy Cast Interrupt";

    public EnemyDeathHeavyCastInterruptState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Interrupt ");

        deathSM.GetDissolveController().StartDissolve(deathSM.etherealDissolveValue);

        if (rewindState.IsRewinding())
        {
            timer = stateTime;
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

        if (timer > stateTime)
        {
            deathSM.SwitchState(new EnemyDeathRestingState(deathSM, false));
        }
    }

    public override void Exit() { }
}
