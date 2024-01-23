using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateMachine : StateMachine
{
    public override void Awake()
    {
        SetupDictionary();
    }

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.idle]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.idle, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.inactive, new BulletInactiveState(this));
        stateDictionary.Add(StateEnum.active, new BulletActiveState(this));
    }

    public void SwitchToActive()
    {
        SwitchState(stateDictionary[StateEnum.active]);
    }
}
