using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActiveState : State
{
    private EnemyMovement enemyMovement;

    public EnemyMeleeActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyMovement = stateMachine.GetComponent<EnemyMovement>();
    }

    public override void Enter()
    {
        enemyMovement.ToggleMovement(true);
        stateMachine.stateMachineAnimator.SetBool("moving", true);
    }

    public override void Exit()
    {
        enemyMovement.ToggleMovement(false);
        stateMachine.stateMachineAnimator.SetBool("moving", false);
    }

    public override void Tick(float deltaTime) { }
}
