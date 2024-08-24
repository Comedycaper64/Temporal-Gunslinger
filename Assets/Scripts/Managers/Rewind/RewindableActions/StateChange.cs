public class StateChange : RewindableAction
{
    private StateMachine stateMachine;
    private State previousState;

    public static void StateChanged(State previousState, StateMachine stateMachine)
    {
        new StateChange(previousState, stateMachine);
    }

    public StateChange(State previousState, StateMachine stateMachine)
    {
        this.previousState = previousState;
        this.stateMachine = stateMachine;

        Execute();
    }

    public override void Undo()
    {
        stateMachine.UndoState(previousState);
    }
}
