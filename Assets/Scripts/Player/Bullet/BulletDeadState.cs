using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeadState : State
{
    Bullet bullet;

    public BulletDeadState(BulletStateMachine stateMachine)
        : base(stateMachine)
    {
        bullet = stateMachine.GetComponent<Bullet>();
    }

    public override void Enter() { }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
