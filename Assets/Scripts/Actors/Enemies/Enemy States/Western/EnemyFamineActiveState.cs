using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFamineActiveState : State
{
    private FamineMovement famineMovement;

    public EnemyFamineActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        famineMovement = stateMachine.GetComponent<FamineMovement>();
    }

    public override void Enter()
    {
        // toggle famine movement
        famineMovement.ResetMovement();
        famineMovement.ToggleMovement(true);
        // toggle famine attacker
    }

    public override void Exit()
    {
        // toggle famine movement
        famineMovement.ResetMovement();
        famineMovement.ToggleMovement(false);
        // toggle famine attacker
    }

    public override void Tick(float deltaTime) { }
}
