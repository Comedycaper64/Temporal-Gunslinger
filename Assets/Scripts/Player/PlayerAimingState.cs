using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    private PlayerController playerController;

    public PlayerAimingState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        //When aiming, ensures mouse is captured
        Cursor.lockState = CursorLockMode.Locked;

        playerController = stateMachine.GetComponent<PlayerController>();
        playerController.TogglePlayerController(true);
        InputManager.Instance.OnShootAction += ShootBullet;
    }

    public override void Tick(float deltaTime)
    {
        // Vector2 mouseMovement =
        //     InputManager.Instance.mouseMovement * mouseSensitivity * Time.deltaTime;

        // xRotation -= mouseMovement.y;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // stateMachine.cameraBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // stateMachine.playerBody.Rotate(Vector3.up * mouseMovement.x);
    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        playerController.TogglePlayerController(false);
        InputManager.Instance.OnShootAction -= ShootBullet;
    }

    private void ShootBullet()
    {
        Transform bulletTransform = Factory.Instance
            .InstantiateGameObject(
                stateMachine.bulletPrefab,
                stateMachine.bulletEmitter.position,
                stateMachine.bulletEmitter.rotation
            )
            .transform;
        stateMachine.SwitchState(new PlayerBulletState(stateMachine, bulletTransform));
    }
}
