using UnityEngine;

public class EnemyEnforcerShootState : State
{
    private float animationTimeNormalised = 0f;

    private EnemyEnforcerStateMachine enemyStateMachine;
    private readonly int ActiveAnimHash;

    public EnemyEnforcerShootState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as EnemyEnforcerStateMachine;
        ActiveAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        stateMachine.stateMachineAnimator.SetBool("shot", true);

        //Debug.Log("Animation Time: " + stateMachine.GetActiveAnimationExitTime());



        if (stateMachine.GetActiveAnimationExitTime() < 0.01f)
        {
            enemyStateMachine.SetProjectileAtFirePoint();
            enemyStateMachine.PlayShootFX();

            BulletStateMachine[] bullets = enemyStateMachine.GetBulletStateMachines();

            foreach (BulletStateMachine bullet in bullets)
            {
                bullet.SwitchToActive();
            }
        }
        else
        {
            stateMachine.stateMachineAnimator.CrossFade(
                ActiveAnimHash,
                0.1f,
                0,
                stateMachine.GetActiveAnimationExitTime()
            );
        }
    }

    public override void Tick(float deltaTime)
    {
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;
    }

    public override void Exit()
    {
        //Debug.Log("Animation exit time: " + animationTimeNormalised);
        stateMachine.SetRunAnimationExitTime(animationTimeNormalised);
        stateMachine.stateMachineAnimator.SetBool("shot", false);
    }
}
