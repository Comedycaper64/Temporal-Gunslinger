using UnityEngine;

public class EnemyEnforcerActiveState : State
{
    EnemyEnforcerStateMachine enemyStateMachine;
    RewindState rewindState;
    private float timer;
    private float shootTime;
    private bool projectileFired;
    private readonly int IdleAnimHash;

    public EnemyEnforcerActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyEnforcerStateMachine;
        shootTime = enemyStateMachine.GetShootTimer();
        rewindState = enemyStateMachine.GetComponent<RewindState>();
        IdleAnimHash = Animator.StringToHash(enemyStateMachine.GetAimingAnimationName());
    }

    public override void Enter()
    {
        timer = enemyStateMachine.GetStateTimerSave();
        projectileFired = false;
        rewindState.ToggleMovement(true);
        stateMachine.stateMachineAnimator.CrossFade(IdleAnimHash, 0.02f);

        // if (rewindState.GetTimeSpeed() < 0f)
        // {
        //     timer = shootTime;
        // }
    }

    public override void Exit()
    {
        enemyStateMachine.SetStateTimerSave(timer);
        rewindState.ToggleMovement(false);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        //Debug.Log("Shoot timer:" + timer);

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;

            stateMachine.SwitchState(new EnemyEnforcerShootState(stateMachine));
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
        }
    }
}
