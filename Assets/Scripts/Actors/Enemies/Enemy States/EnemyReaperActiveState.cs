using UnityEngine;

public class EnemyReaperActiveState : State
{
    private ReaperMovement reaperMovement;
    private RepeatingMaskSpawner repeatingMaskSpawner;

    public EnemyReaperActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        reaperMovement = stateMachine.GetComponent<ReaperMovement>();
        repeatingMaskSpawner = stateMachine.GetComponent<RepeatingMaskSpawner>();
    }

    public override void Enter()
    {
        reaperMovement.ToggleMovement(true);
        repeatingMaskSpawner.ToggleMovement(true);
    }

    public override void Exit()
    {
        reaperMovement.ToggleMovement(false);
        repeatingMaskSpawner.ToggleMovement(false);
    }

    public override void Tick(float deltaTime) { }
}
