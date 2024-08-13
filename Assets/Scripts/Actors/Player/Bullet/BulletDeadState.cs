using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeadState : State
{
    public static int bulletNumber;
    Bullet bullet;

    public BulletDeadState(BulletStateMachine stateMachine)
        : base(stateMachine)
    {
        bullet = stateMachine.GetComponent<Bullet>();
        bulletNumber++;
    }

    public override void Enter()
    {
        bulletNumber--;
        bullet.SetIsDead(true);
        //stateMachine.ToggleInactive(true);
        if (bulletNumber <= 0)
        {
            GameManager.Instance.LevelLost();
        }
    }

    public override void Exit()
    {
        bulletNumber++;
        bullet.SetIsDead(false);
        //stateMachine.ToggleInactive(false);
        if (bulletNumber <= 1)
        {
            GameManager.Instance.UndoLevelLost();
        }
    }

    public override void Tick(float deltaTime) { }
}
