public class BlankState : State
{
    public BlankState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter() { }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
