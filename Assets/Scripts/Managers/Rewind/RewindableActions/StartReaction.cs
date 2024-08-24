public class StartReaction : RewindableAction
{
    private IReactable reactable;

    public static void ReactionStarted(IReactable reactable)
    {
        new StartReaction(reactable);
    }

    private StartReaction(IReactable reactable)
    {
        this.reactable = reactable;

        Execute();
    }

    public override void Undo()
    {
        reactable.UndoReaction();
    }
}
