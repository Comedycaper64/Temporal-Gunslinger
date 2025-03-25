using UnityEngine;

public class EnemyTurretIdleState : State
{
    private Animator animator;

    private RepeatingBulletSpawner turretBulletSpawner;

    public EnemyTurretIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
        turretBulletSpawner = stateMachine.GetComponent<RepeatingBulletSpawner>();
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("activate");
        }

        turretBulletSpawner.ResetSpawner();
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
