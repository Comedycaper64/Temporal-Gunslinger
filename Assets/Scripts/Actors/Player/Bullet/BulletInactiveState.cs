using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInactiveState : State
{
    Bullet bullet;

    public BulletInactiveState(BulletStateMachine stateMachine)
        : base(stateMachine)
    {
        bullet = stateMachine.GetComponent<Bullet>();
    }

    public override void Enter()
    {
        bullet.ToggleBulletActive(false);
        //bullet.ToggleBulletPossessed(false);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
