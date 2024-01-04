using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateMachine : StateMachine
{
    //public static event EventHandler<State> OnPlayerStateChanged;

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    // public override void SwitchState(State newState)
    // {
    //     base.SwitchState(newState);
    //     OnPlayerStateChanged?.Invoke(this, newState);
    // }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new PlayerAimingState(this));
        stateDictionary.Add(StateEnum.active, new PlayerBulletState(this));
    }
}
