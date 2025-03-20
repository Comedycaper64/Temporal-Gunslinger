using UnityEngine;

public class EnemyReaperActiveState : State
{
    Animator animator;
    private ReaperMovement reaperMovement;
    private RepeatingMaskSpawner repeatingMaskSpawner;

    public EnemyReaperActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
        reaperMovement = stateMachine.GetComponent<ReaperMovement>();
        repeatingMaskSpawner = stateMachine.GetComponent<RepeatingMaskSpawner>();
    }

    public override void Enter()
    {
        reaperMovement.ToggleMovement(true);
        repeatingMaskSpawner.ToggleMovement(true);
        animator.SetBool("flee", true);
    }

    public override void Exit()
    {
        reaperMovement.ToggleMovement(false);
        repeatingMaskSpawner.ToggleMovement(false);
        animator.SetBool("flee", false);
    }

    public override void Tick(float deltaTime) { }
}
