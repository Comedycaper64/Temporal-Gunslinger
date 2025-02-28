using UnityEngine;

public class EnemyDeathDeadzonesState : State
{
    private float timer = 0f;
    private float stateTime = 0.024f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;
    private DeathDeadzone[] deadzones;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Conjure Deadzones";

    public EnemyDeathDeadzonesState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
        deadzones = new DeathDeadzone[]
        {
            deathSM.GetDeathDeadzone(),
            deathSM.GetDeathDeadzone(),
            deathSM.GetDeathDeadzone()
        };
    }

    public override void Enter()
    {
        Debug.Log(" Deadzone Pattern ");

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

            foreach (DeathDeadzone deadzone in deadzones)
            {
                deadzone.ToggleMovement(true);
            }
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

    public override void Exit()
    {
        if (rewindState.IsRewinding())
        {
            foreach (DeathDeadzone deadzone in deadzones)
            {
                deadzone.ToggleMovement(false);
            }
        }
    }
}
