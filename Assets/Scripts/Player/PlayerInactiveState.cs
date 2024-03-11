using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInactiveState : State
{
    //Animator animator;
    private PlayerController playerController;

    public PlayerInactiveState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerController = stateMachine.GetComponent<PlayerController>();
        //animator = stateMachine.stateMachineAnimator;
    }

    public override void Enter()
    {
        //When aiming, ensures mouse is captured
        //Cursor.lockState = CursorLockMode.Locked;
        //playerController.DisableGun();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        //Cursor.lockState = CursorLockMode.None;
        //playerController.TogglePlayerController(false);
    }
}
