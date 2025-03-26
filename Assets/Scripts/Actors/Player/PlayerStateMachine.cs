using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField]
    private Transform revenantChest;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new PlayerInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new PlayerAimingState(this));
        stateDictionary.Add(StateEnum.active, new PlayerBulletState(this));
        stateDictionary.Add(StateEnum.dead, new PlayerDeadState(this));
    }

    public override void ToggleInactive(bool toggle) { }

    protected override void DebugKill() { }

    public Transform GetRevenantChest()
    {
        return revenantChest;
    }
}
