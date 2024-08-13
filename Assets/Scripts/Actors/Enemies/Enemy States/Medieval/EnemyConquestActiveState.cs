using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConquestActiveState : State
{
    EnemyConquestStateMachine conquestStateMachine;
    RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;
    private float animationSpeedMult = 5f;

    public EnemyConquestActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        conquestStateMachine = stateMachine as EnemyConquestStateMachine;
        shootTime = conquestStateMachine.GetShootTimer();
        rewindState = conquestStateMachine.GetComponent<RewindState>();
    }

    public override void Enter()
    {
        timer = 0f;
        projectileFired = false;
        rewindState.ToggleMovement(true);
        stateMachine.stateMachineAnimator.SetBool("shoot", true);

        //make projectile bullet holder appear
        conquestStateMachine.ToggleBulletHolder(true);
        conquestStateMachine.SetBulletAnimationTime(0f);
        conquestStateMachine.SetSwordAnimatorPlay();
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
        stateMachine.stateMachineAnimator.SetBool("shoot", false);
        conquestStateMachine.ToggleBulletHolder(false);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed() * animationSpeedMult;

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;
            conquestStateMachine.GetBulletStateMachine().SwitchToActive();
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
            stateMachine.StartCoroutine(DelayedBulletEnable());
            //set projectile animation to be at the end of the clip
        }
    }

    private IEnumerator DelayedBulletEnable()
    {
        yield return null;
        conquestStateMachine.ToggleBulletHolder(true);
        conquestStateMachine.SetBulletAnimationTime(1f);
    }
}
