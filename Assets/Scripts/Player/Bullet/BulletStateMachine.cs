using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateMachine : StateMachine
{
    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.active, new BulletActiveState(this));
    }
}
