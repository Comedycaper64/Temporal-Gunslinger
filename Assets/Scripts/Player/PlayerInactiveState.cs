using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInactiveState : State
{
    private PlayerController playerController;

    public PlayerInactiveState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        //When aiming, ensures mouse is captured
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        //Cursor.lockState = CursorLockMode.None;
        //playerController.TogglePlayerController(false);
    }
}
