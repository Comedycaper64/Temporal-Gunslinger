using UnityEngine;

public class BulletStateMachine : StateMachine
{
    [SerializeField]
    public bool spawnedBullet;

    [SerializeField]
    public bool bCountAsAvailableBullet = true;

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.idle]);
    }

    protected override void DebugKill() { }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.idle, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.inactive, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.active, new BulletActiveState(this));
        stateDictionary.Add(StateEnum.dead, new BulletDeadState(this));
    }

    public void SwitchToActive()
    {
        // if (GetCurrentState().GetType() == typeof(BulletActiveState))
        // {
        //     return;
        // }
        if (!GameManager.bLevelActive)
        {
            return;
        }

        SwitchState(stateDictionary[StateEnum.active]);
    }

    public void SwitchToInactive()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    public override void GameManager_OnGameStateChange(object sender, StateEnum stateChange)
    {
        if (stateChange != StateEnum.inactive)
        {
            return;
        }

        base.GameManager_OnGameStateChange(sender, stateChange);
    }

    public override void ToggleInactive(bool toggle) { }
}
