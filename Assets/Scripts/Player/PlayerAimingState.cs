using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : State
{
    private PlayerController playerController;

    public PlayerAimingState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        //When aiming, ensures mouse is captured
        Cursor.lockState = CursorLockMode.Locked;

        playerController.TogglePlayerController(true);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        playerController.TogglePlayerController(false);
    }
}
