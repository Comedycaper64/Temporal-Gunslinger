using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDeadState : State
{
    public static event EventHandler<bool> OnPlayerDied;

    public PlayerDeadState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        //Retry UI
        TimeManager.SetPausedTime();
        OnPlayerDied?.Invoke(this, true);
    }

    public override void Exit()
    {
        OnPlayerDied?.Invoke(this, false);
    }

    public override void Tick(float deltaTime) { }
}
