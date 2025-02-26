public class RewindState : RewindableMovement
{
    public float GetTimeSpeed() => GetUnscaledSpeed();

    public float GetScaledSpeed() => GetSpeed();

    public bool IsRewinding()
    {
        return GetUnscaledSpeed() < 0f;
    }
}
