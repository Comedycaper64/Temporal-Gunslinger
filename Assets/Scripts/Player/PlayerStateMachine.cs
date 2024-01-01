using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public GameObject bulletPrefab;

    // public Transform playerBody;
    // public Transform cameraBody;
    public Transform bulletEmitter;

    private void Start()
    {
        SwitchState(new PlayerAimingState(this));
    }
}
