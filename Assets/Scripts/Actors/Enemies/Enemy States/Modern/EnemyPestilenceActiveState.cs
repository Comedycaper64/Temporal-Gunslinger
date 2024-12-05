using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPestilenceActiveState : State
{
    private EnemyPestilenceStateMachine pestilenceStateMachine;
    private RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;
    private float animationSpeedMult = 5f;

    public EnemyPestilenceActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        pestilenceStateMachine = stateMachine as EnemyPestilenceStateMachine;
        shootTime = pestilenceStateMachine.GetShootTimer();
        rewindState = pestilenceStateMachine.GetComponent<RewindState>();
    }

    public override void Enter()
    {
        timer = 0f;
        projectileFired = false;
        rewindState.ToggleMovement(true);
        stateMachine.stateMachineAnimator.SetBool("shot", true);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed() * animationSpeedMult;

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;
            pestilenceStateMachine.shotArrow.SetActive(true);
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
            pestilenceStateMachine.shotArrow.SetActive(false);
        }
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
        pestilenceStateMachine.shotArrow.SetActive(false);
        stateMachine.stateMachineAnimator.SetBool("shot", false);
    }
}
