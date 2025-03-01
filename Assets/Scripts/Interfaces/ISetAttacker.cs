using System;

public interface ISetAttacker
{
    event EventHandler<bool> OnAttackToggled;
    event EventHandler<float> OnTimeOffset;

    public void ToggleAttack(bool toggle);

    public void SetTimeOffset(float offset);
}
