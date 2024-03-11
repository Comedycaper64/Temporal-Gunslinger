using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateMachine : StateMachine
{
    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.idle]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.idle, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.inactive, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.active, new BulletActiveState(this));
        stateDictionary.Add(StateEnum.dead, new BulletDeadState(this));
    }

    public void SwitchToActive()
    {
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
