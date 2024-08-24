public class PlayerDeadState : State
{
    public PlayerDeadState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        GameManager.Instance.LevelLost();
    }

    public override void Exit()
    {
        GameManager.Instance.UndoLevelLost();
    }

    public override void Tick(float deltaTime) { }
}
