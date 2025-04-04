using UnityEngine;

public class EnemyMaskShootState : State
{
    private EnemyRangedStateMachine enemyStateMachine;
    private MaskStateMachine maskStateMachine;
    private RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;

    public EnemyMaskShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyRangedStateMachine;
        maskStateMachine = stateMachine as MaskStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
    }

    public override void Enter()
    {
        timer = enemyStateMachine.GetStateTimerSave();
        projectileFired = false;
        rewindState.ToggleMovement(true);

        if (timer >= shootTime)
        {
            projectileFired = true;
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;

            enemyStateMachine.SetProjectileAtFirePoint();
            enemyStateMachine.GetBulletStateMachine().SwitchToActive();
            AudioManager.PlaySFX(
                maskStateMachine.GetFireSFX(),
                0.75f,
                0,
                stateMachine.transform.position
            );
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }

    public override void Exit()
    {
        enemyStateMachine.SetStateTimerSave(timer);
        rewindState.ToggleMovement(false);
    }
}
