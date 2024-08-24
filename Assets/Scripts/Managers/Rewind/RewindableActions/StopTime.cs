public class StopTime : RewindableAction
{
    public static void TimeStopped()
    {
        new StopTime();
    }

    public StopTime()
    {
        Execute();
    }

    public override void Undo()
    {
        TimeManager.UnpauseTime();
    }

    public override bool IsPriority()
    {
        return true;
    }
}
