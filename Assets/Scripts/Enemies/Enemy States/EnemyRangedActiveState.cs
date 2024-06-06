using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedActiveState : State
{
    EnemyRangedStateMachine enemyStateMachine;
    RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;

    public EnemyRangedActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyRangedStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
    }

    public override void Enter()
    {
        timer = 0f;
        projectileFired = false;
        rewindState.ToggleMovement(true);
        if (rewindState.GetTimeSpeed() < 0f)
        {
            timer = shootTime;
            projectileFired = true;
        }
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetTimeSpeed();

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;

            stateMachine.SwitchState(new EnemyRangedShootState(stateMachine));
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }
}
