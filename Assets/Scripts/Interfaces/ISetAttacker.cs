using System;

public interface ISetAttacker
{
    event EventHandler<bool> OnAttackToggled;

    public void ToggleAttack(bool toggle);
}
