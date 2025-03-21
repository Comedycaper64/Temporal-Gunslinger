using UnityEngine;

public class EnemyMaskBossStateMachine : EnemyRangedStateMachine
{
    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyBossMaskShootState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public virtual void SwitchToActiveState()
    {
        SwitchState(stateDictionary[StateEnum.active]);
    }

    public override void ToggleInactive(bool toggle)
    {
        base.ToggleInactive(toggle);

        stateMachineAnimator.SetBool("deactivate", toggle);
    }

    public override void GameManager_OnGameStateChange(object sender, StateEnum stateChange)
    {
        if (stateChange == StateEnum.active)
        {
            return;
        }

        base.GameManager_OnGameStateChange(sender, stateChange);
    }
}
