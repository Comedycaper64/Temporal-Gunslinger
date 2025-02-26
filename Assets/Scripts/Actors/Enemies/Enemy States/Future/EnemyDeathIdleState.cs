public class EnemyDeathIdleState : State
{
    private EnemyDeathStateMachine deathSM;
    private RewindState rewindState;

    public EnemyDeathIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
    }

    public override void Enter()
    {
        rewindState.ToggleMovement(true);
        deathSM.ResetFlow();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
