using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaskShootState : State
{
    EnemyRangedStateMachine enemyStateMachine;

    public EnemyMaskShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyRangedStateMachine;
    }

    public override void Enter()
    {
        enemyStateMachine.SetProjectileAtFirePoint();
        enemyStateMachine.GetBulletStateMachine().SwitchToActive();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
