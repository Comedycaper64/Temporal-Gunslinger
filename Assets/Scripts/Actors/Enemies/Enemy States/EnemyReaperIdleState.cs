using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaperIdleState : State
{
    Animator animator;
    private EnemyReaperStateMachine reaperStateMachine;

    public EnemyReaperIdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
        animator = stateMachine.stateMachineAnimator;
        reaperStateMachine = stateMachine as EnemyReaperStateMachine;
    }

    public override void Enter()
    {
        if (animator)
        {
            animator.SetTrigger("activate");
        }

        if (!reaperStateMachine.HasStartPosition())
        {
            Transform newTransform = new GameObject("Enemy Start Pos").transform;
            newTransform.position = reaperStateMachine.transform.position;
            reaperStateMachine.SetStartPosition(newTransform);
        }

        reaperStateMachine.ResetPosition();
        reaperStateMachine.ResetMaskSpawner();
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
