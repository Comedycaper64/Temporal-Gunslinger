using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActiveState : State
{
    EnemyStateMachine enemyStateMachine;
    RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;

    public EnemyActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
    }

    public override void Enter()
    {
        timer = 0f;
        projectileFired = false;
        rewindState.ToggleMovement(true);
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.unscaledDeltaTime * rewindState.GetTimeSpeed();

        if (!projectileFired && timer >= shootTime)
        {
            enemyStateMachine.GetBulletStateMachine().SwitchToActive();
            projectileFired = true;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }
}
