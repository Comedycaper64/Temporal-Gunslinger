using System;
using UnityEngine;

public class EnemyBossDeadState : State
{
    private readonly int DeathAnimHash;
    private Animator animator;
    private DissolveController dissolveController;
    public static Action OnBossDeadACH;

    public EnemyBossDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        EnemyDeadState.enemiesAlive++;
        animator = stateMachine.stateMachineAnimator;
        dissolveController = stateMachine.GetDissolveController();
        DeathAnimHash = Animator.StringToHash(stateMachine.GetActiveAnimationName());
    }

    public override void Enter()
    {
        OnBossDeadACH?.Invoke();
        animator.SetBool("death", true);
        animator.CrossFade(DeathAnimHash, 0.02f);
        dissolveController.StartDissolve(0.25f);
        EnemyDeadState.enemiesAlive = 0;

        stateMachine.ToggleInactive(true);

        //disable all enemies

        GameManager.Instance.EndLevel(stateMachine.transform);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
