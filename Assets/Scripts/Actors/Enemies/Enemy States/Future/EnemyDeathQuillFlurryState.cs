public class EnemyDeathQuillFlurryState : State
{
    private EnemyDeathStateMachine deathSM;

    public EnemyDeathQuillFlurryState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
    }

    public override void Enter() { }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
