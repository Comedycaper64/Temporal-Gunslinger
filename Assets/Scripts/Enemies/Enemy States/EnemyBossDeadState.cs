using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossDeadState : State
{
    private Animator animator;
    private DissolveController dissolveController;

    public EnemyBossDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        EnemyDeadState.enemiesAlive++;
        animator = stateMachine.stateMachineAnimator;
        dissolveController = stateMachine.dissolveController;
    }

    public override void Enter()
    {
        animator.SetBool("death", true);
        dissolveController.StartDissolve(0.5f);
        EnemyDeadState.enemiesAlive = 0;

        //disable all enemies

        GameManager.Instance.EndLevel(stateMachine.transform);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
