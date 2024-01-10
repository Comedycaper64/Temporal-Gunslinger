using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActiveState : State
{
    EnemyMovement enemyMovement;

    public EnemyMeleeActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyMovement = stateMachine.GetComponent<EnemyMovement>();
    }

    public override void Enter()
    {
        enemyMovement.ToggleMovement(true);
    }

    public override void Exit()
    {
        enemyMovement.ToggleMovement(false);
    }

    public override void Tick(float deltaTime) { }
}
