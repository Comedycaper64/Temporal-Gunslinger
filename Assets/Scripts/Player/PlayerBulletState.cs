using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletState : State
{
    //Transform bulletTransform;

    public PlayerBulletState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        //bulletTransform = bullet;
    }

    public override void Enter()
    {
        //When controlling bullet, ensures mouse is captured
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // public Transform GetBulletTransform()
    // {
    //     return bulletTransform;
    // }
}
