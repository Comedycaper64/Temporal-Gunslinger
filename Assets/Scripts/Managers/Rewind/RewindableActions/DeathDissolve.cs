public class DeathDissolve : RewindableAction
{
    private bool start;
    private float previousStart;
    private float previousTarget;
    private float dissolveCounter;
    private DeathDissolveController dissolveController;

    public static void Dissolved(
        DeathDissolveController controller,
        float previousStart,
        float previousTarget,
        float dissolveCounter,
        bool start
    )
    {
        new DeathDissolve(controller, previousStart, previousTarget, dissolveCounter, start);
    }

    public DeathDissolve(
        DeathDissolveController controller,
        float previousStart,
        float previousTarget,
        float dissolveCounter,
        bool start
    )
    {
        dissolveController = controller;
        this.previousStart = previousStart;
        this.previousTarget = previousTarget;
        this.dissolveCounter = dissolveCounter;
        this.start = start;

        // Debug.Log(
        //     "Start: "
        //         + previousStart
        //         + " Target: "
        //         + previousTarget
        //         + " Counter: "
        //         + dissolveCounter
        // );

        Execute();
    }

    public override void Undo()
    {
        // Debug.Log(
        //     " Undo Start: "
        //         + previousStart
        //         + " Target: "
        //         + previousTarget
        //         + " Counter: "
        //         + dissolveCounter
        // );

        if (start)
        {
            dissolveController.UndoDissolve(previousStart);
        }
        else
        {
            dissolveController.UndoDissolveEnd(previousStart, previousTarget, dissolveCounter);
        }
    }
}
