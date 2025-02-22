public class EnemyDeathHeavyCastInterruptState : State
{
    private EnemyDeathStateMachine deathSM;

    public EnemyDeathHeavyCastInterruptState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
    }

    public override void Enter() { }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
