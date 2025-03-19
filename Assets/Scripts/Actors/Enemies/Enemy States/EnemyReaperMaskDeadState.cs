using System;
using UnityEngine;

public class EnemyReaperMaskDeadState : State
{
    protected DissolveController dissolveController;

    public static EventHandler<Transform> OnReaperMaskKilled;

    public EnemyReaperMaskDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        dissolveController = stateMachine.GetDissolveController();
    }

    public override void Enter()
    {
        if (dissolveController)
        {
            dissolveController.StartDissolve();
        }

        EnemyDeadState.enemiesAlive = 0;

        var tempGameObject = new GameObject("Level Pos");
        tempGameObject.transform.position = new Vector3(0f, 1f, 5f);

        OnReaperMaskKilled?.Invoke(this, tempGameObject.transform);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
