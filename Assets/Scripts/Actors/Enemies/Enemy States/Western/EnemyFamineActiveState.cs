public class EnemyFamineActiveState : State
{
    private FamineMovement famineMovement;
    private FamineLocustSpawner famineLocustSpawner;

    public EnemyFamineActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        famineMovement = stateMachine.GetComponent<FamineMovement>();
        famineLocustSpawner = stateMachine.GetComponent<FamineLocustSpawner>();
    }

    public override void Enter()
    {
        // toggle famine movement
        famineMovement.ResetMovement();
        famineMovement.ToggleMovement(true);

        famineLocustSpawner.ResetSpawner();
        famineLocustSpawner.ToggleMovement(true);
        // toggle famine attacker
    }

    public override void Exit()
    {
        // toggle famine movement
        famineMovement.ResetMovement();
        famineMovement.ToggleMovement(false);

        famineLocustSpawner.ResetSpawner();
        famineLocustSpawner.ToggleMovement(false);
        // toggle famine attacker
    }

    public override void Tick(float deltaTime) { }
}
