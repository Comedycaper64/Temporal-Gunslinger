using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletState : PlayerBaseState
{
    float xRotation = 0f;
    private float mouseSensitivity = 15f;
    Bullet bullet;

    public PlayerBulletState(PlayerStateMachine stateMachine, Bullet bullet)
        : base(stateMachine)
    {
        this.bullet = bullet;
    }

    public override void Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.OnShootAction += RedirectBullet;
    }

    public override void Tick(float deltaTime)
    {
        // Vector2 mouseMovement =
        //     InputManager.Instance.mouseMovement * mouseSensitivity * Time.deltaTime;

        // xRotation -= mouseMovement.y;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // bullet.camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        InputManager.Instance.OnShootAction -= RedirectBullet;
    }

    private void RedirectBullet()
    {
        bullet.RedirectBullet();
    }
}
