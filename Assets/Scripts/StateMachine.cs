using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public virtual void SwitchState(State newState)
    {
        StateChange.StateChanged(currentState, this);
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
