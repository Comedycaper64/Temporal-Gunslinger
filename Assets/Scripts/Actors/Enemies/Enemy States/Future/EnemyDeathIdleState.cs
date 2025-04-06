using UnityEngine;

public class EnemyDeathIdleState : State
{
    private EnemyDeathStateMachine deathSM;
    private RewindState rewindState;
    private Animator animator;

    public EnemyDeathIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        animator = stateMachine.stateMachineAnimator;
        rewindState = deathSM.GetRewindState();
    }

    public override void Enter()
    {
        rewindState.ToggleMovement(true);
        animator.SetTrigger("activate");
        deathSM.ResetFlow();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
