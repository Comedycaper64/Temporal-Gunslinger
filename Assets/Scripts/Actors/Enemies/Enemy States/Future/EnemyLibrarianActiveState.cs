using UnityEngine;

public class EnemyLibrarianActiveState : State
{
    private EnemyLibrarianStateMachine librarianStateMachine;
    private RewindState rewindState;
    private float timer;
    private float shootTime;
    private float animationTimeNormalised = 0f;
    private bool projectileFired;
    private readonly int ActiveAnimHash;

    public EnemyLibrarianActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        librarianStateMachine = stateMachine as EnemyLibrarianStateMachine;
        shootTime = librarianStateMachine.GetShootTimer();
        rewindState = librarianStateMachine.GetComponent<RewindState>();
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        timer = librarianStateMachine.GetStateTimerSave();
        rewindState.ToggleMovement(true);
        stateMachine.stateMachineAnimator.SetBool("shot", true);

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

        if (!projectileFired && timer >= shootTime)
        {
            projectileFired = true;
            librarianStateMachine.shotArrow.SetActive(true);
            return;
        }
        else if (projectileFired && timer < shootTime)
        {
            projectileFired = false;
            librarianStateMachine.shotArrow.SetActive(false);
        }
    }

    public override void Exit()
    {
        rewindState.ToggleMovement(false);
        librarianStateMachine.shotArrow.SetActive(false);
        stateMachine.stateMachineAnimator.SetBool("shot", false);
        stateMachine.SetRunAnimationExitTime(animationTimeNormalised);
        librarianStateMachine.SetStateTimerSave(timer);
    }
}
