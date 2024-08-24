using UnityEngine;

public class EnemyMeleeActiveState : State
{
    private float animationTimeNormalised;
    private EnemyMovement enemyMovement;
    private readonly int ActiveAnimHash;

    public EnemyMeleeActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyMovement = stateMachine.GetComponent<EnemyMovement>();
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        enemyMovement.ToggleMovement(true);
        stateMachine.stateMachineAnimator.SetBool("moving", true);
        stateMachine.stateMachineAnimator.CrossFade(
            ActiveAnimHash,
            0.02f,
            0,
            stateMachine.GetActiveAnimationExitTime()
        );
    }

    public override void Tick(float deltaTime)
    {
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;
    }

    public override void Exit()
    {
        enemyMovement.ToggleMovement(false);
        stateMachine.stateMachineAnimator.SetBool("moving", false);
        stateMachine.SetRunAnimationExitTime(animationTimeNormalised);
    }
}
