using System;
using UnityEngine;

public class CrystalDeadState : State
{
    protected DissolveController dissolveController;

    public static Action OnCrystalDeadACH;

    public CrystalDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        dissolveController = stateMachine.GetDissolveController();
    }

    public override void Enter()
    {
        if (dissolveController)
        {
            dissolveController.StartDissolve();
        }

        stateMachine.ToggleInactive(true);
        OnCrystalDeadACH?.Invoke();
    }

    public override void Exit()
    {
        if (dissolveController)
        {
            dissolveController.StopDissolve();
        }

        stateMachine.ToggleInactive(false);
    }

    public override void Tick(float deltaTime) { }
}
