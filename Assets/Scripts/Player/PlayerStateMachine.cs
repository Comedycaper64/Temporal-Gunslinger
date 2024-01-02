using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public GameObject bulletPrefab;

    // public Transform playerBody;
    // public Transform cameraBody;
    public Transform bulletEmitter;

    public static event EventHandler<State> OnPlayerStateChanged;

    private void Start()
    {
        SwitchState(new PlayerAimingState(this));
    }

    public override void SwitchState(State newState)
    {
        base.SwitchState(newState);
        OnPlayerStateChanged?.Invoke(this, newState);
    }
}
