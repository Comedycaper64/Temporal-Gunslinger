public class PlayerInactiveState : State
{
    private PlayerController playerController;

    public PlayerInactiveState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        playerController.ResetPlayerRotation();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
