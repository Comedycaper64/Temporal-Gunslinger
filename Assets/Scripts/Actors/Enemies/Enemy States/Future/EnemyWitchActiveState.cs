using UnityEngine;

public class EnemyWitchActiveState : State
{
    private EnemyWitchStateMachine witchStateMachine;

    private RewindState rewindState;

    private readonly int ActiveAnimHash;
    private float animationTimeNormalised = 0f;
    private float timer;

    public EnemyWitchActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        witchStateMachine = stateMachine as EnemyWitchStateMachine;
        rewindState = witchStateMachine.GetComponent<RewindState>();
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        timer = witchStateMachine.GetStateTimerSave();
        stateMachine.stateMachineAnimator.SetBool("shot", true);
        rewindState.ToggleMovement(true);

        if (stateMachine.GetActiveAnimationExitTime() >= 0.01f)
        {
            stateMachine.stateMachineAnimator.CrossFade(
                ActiveAnimHash,
                0.1f,
                0,
                stateMachine.GetActiveAnimationExitTime()
            );
        }
        else
        {
            stateMachine.stateMachineAnimator.CrossFade(ActiveAnimHash, 0.02f);
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;

        CheckBullet(0);
        CheckBullet(1);
        CheckBullet(2);
    }

    private void CheckBullet(int bulletIndex)
    {
        if (
            !witchStateMachine.GetBulletsFired()[bulletIndex]
            && (timer >= witchStateMachine.GetShootTimes()[bulletIndex])
        )
        {
            witchStateMachine.FireBullet(bulletIndex);
        }
        else if (
            witchStateMachine.GetBulletsFired()[bulletIndex]
            && (timer < witchStateMachine.GetShootTimes()[bulletIndex])
        )
        {
            witchStateMachine.UnfireBullet(bulletIndex);
        }
    }

    public override void Exit()
    {
        stateMachine.SetRunAnimationExitTime(animationTimeNormalised);
        stateMachine.stateMachineAnimator.SetBool("shot", false);
        witchStateMachine.SetStateTimerSave(timer);
        rewindState.ToggleMovement(false);
    }
}
