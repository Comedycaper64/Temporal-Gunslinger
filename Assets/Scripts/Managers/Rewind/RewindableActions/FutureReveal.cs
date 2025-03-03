using UnityEngine;

public class FutureReveal : RewindableAction
{
    private float previousRevealState = 0f;
    private Vector3 revealPos;
    private FutureEnvironment futureEnvironment;

    public static void NewReveal(FutureEnvironment env, float prevReveal, Vector3 revealPos)
    {
        new FutureReveal(env, prevReveal, revealPos);
    }

    public FutureReveal(FutureEnvironment env, float prevReveal, Vector3 revealPos)
    {
        previousRevealState = prevReveal;
        futureEnvironment = env;
        this.revealPos = revealPos;
        Execute();
    }

    public override void Undo()
    {
        futureEnvironment.UndoReveal(previousRevealState, revealPos);
    }
}
