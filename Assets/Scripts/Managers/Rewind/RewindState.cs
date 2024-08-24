public class RewindState : RewindableMovement
{
    public float GetTimeSpeed() => GetUnscaledSpeed();

    public float GetScaledSpeed() => GetSpeed();
}
