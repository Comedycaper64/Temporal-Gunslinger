using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    float xRotation = 0f;
    private float mouseSensitivity = 15f;

    public PlayerAimingState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.OnShootAction += ShootBullet;
    }

    public override void Tick(float deltaTime)
    {
        Vector2 mouseMovement =
            InputManager.Instance.mouseMovement * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseMovement.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        stateMachine.cameraBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        stateMachine.playerBody.Rotate(Vector3.up * mouseMovement.x);
    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        InputManager.Instance.OnShootAction -= ShootBullet;
    }

    private void ShootBullet()
    {
        Bullet bullet = GameObject
            .Instantiate(
                stateMachine.bulletPrefab,
                stateMachine.bulletEmitter.position,
                Quaternion.Euler(stateMachine.bulletEmitter.forward)
            )
            .GetComponent<Bullet>();
        bullet.SetupBullet(1f);
        stateMachine.SwitchState(new PlayerBulletState(stateMachine, bullet));
    }
}
