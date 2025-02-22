using System;

public class DeathHealth : ActorHealth
{
    public Action OnDamageTaken;

    protected override void Die(object sender, EventArgs e)
    {
        OnDamageTaken?.Invoke();
    }
}
