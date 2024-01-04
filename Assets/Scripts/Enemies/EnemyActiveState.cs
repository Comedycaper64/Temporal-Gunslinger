using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActiveState : State
{
    public EnemyActiveState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        EnemyStateMachine enemyStateMachine = stateMachine as EnemyStateMachine;
        Factory.InstantiateGameObject(enemyStateMachine.projectile, enemyStateMachine.emitter);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
