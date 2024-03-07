using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletState : State
{
    Animator animator;
    private PlayerController playerController;

    public PlayerBulletState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
        playerController = stateMachine.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        //When controlling bullet, ensures mouse is captured
        Cursor.lockState = CursorLockMode.Locked;
        playerController.ToggleBulletFired(true);
        animator.SetBool("shot", true);
        BulletVelocityUI.Instance.ToggleUIActive(true);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        playerController.ToggleBulletFired(false);
        animator.SetBool("shot", false);
        BulletVelocityUI.Instance.ToggleUIActive(false);
    }

    // public Transform GetBulletTransform()
    // {
    //     return bulletTransform;
    // }
}
