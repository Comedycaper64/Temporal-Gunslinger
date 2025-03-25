using UnityEngine;

public class EnemyTurretActiveState : State
{
    private readonly int ActiveAnimHash;
    private RepeatingBulletSpawner turretBulletSpawner;

    public EnemyTurretActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        turretBulletSpawner = stateMachine.GetComponent<RepeatingBulletSpawner>();
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        stateMachine.stateMachineAnimator.SetBool("shot", true);

        turretBulletSpawner.ToggleMovement(true);

        float enterTime = turretBulletSpawner.GetSpawnTimer() / turretBulletSpawner.GetSpawnTime();

        stateMachine.stateMachineAnimator.CrossFade(ActiveAnimHash, 0.01f, 0, enterTime);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        stateMachine.stateMachineAnimator.SetBool("shot", false);

        turretBulletSpawner.ToggleMovement(false);
    }
}
