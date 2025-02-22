public class EnemyDeathRestingState : State
{
    private bool teleport;
    private float restTime = 0.02f;
    private EnemyDeathStateMachine deathSM;

    public EnemyDeathRestingState(StateMachine stateMachine, bool teleport)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        this.teleport = teleport;
    }

    public override void Enter()
    {
        //If teleport, start off dissolves, teleport to rest point, then undissolve
        if (teleport) { }
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
