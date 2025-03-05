using UnityEngine;

public class EnemyPolterThrowState : State
{
    EnemyPolterStateMachine enemyStateMachine;
    RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;
    private readonly int ThrowAnimHash;

    public EnemyPolterThrowState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyPolterStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
        ThrowAnimHash = Animator.StringToHash(enemyStateMachine.GetAimingAnimationName());
    }

    public override void Enter()
    {
        projectileFired = false;
        rewindState.ToggleMovement(true);
        enemyStateMachine.PlayShootFX();
        timer = 0f;

        stateMachine.stateMachineAnimator.CrossFade(ThrowAnimHash, 0.02f);
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        //Debug.Log("Shoot timer:" + timer);

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;

            enemyStateMachine.SetProjectileAtFirePoint();
            enemyStateMachine.GetBulletStateMachine().SwitchToActive();
            enemyStateMachine.LockOnBullet();
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }
}
