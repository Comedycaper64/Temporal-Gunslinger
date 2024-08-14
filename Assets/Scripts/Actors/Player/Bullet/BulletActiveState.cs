using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletActiveState : State
{
    Bullet bullet;

    //private BulletStateMachine bulletStateMachine;
    public BulletActiveState(BulletStateMachine stateMachine)
        : base(stateMachine)
    {
        bullet = stateMachine.GetComponent<Bullet>();
        //bulletStateMachine = stateMachine;
    }

    public override void Enter()
    {
        bullet.ToggleBulletActive(true);
    }

    public override void Exit()
    {
        //bullet.ToggleBulletActive(false);
    }

    public override void Tick(float deltaTime) { }
}
