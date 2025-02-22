public class EnemyDeathScytheAState : State
{
    private EnemyDeathStateMachine deathSM;

    public EnemyDeathScytheAState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
    }

    public override void Enter() { }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
