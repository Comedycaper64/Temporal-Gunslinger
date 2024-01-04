using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    protected Dictionary<StateEnum, State> stateDictionary = new Dictionary<StateEnum, State>();

    private void Awake()
    {
        SetupDictionary();
        GameManager.OnGameStateChange += GameManagerStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameManagerStateChange;
    }

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

    public virtual void UndoState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    protected abstract void SetupDictionary();

    private void GameManagerStateChange(object sender, StateEnum stateChange)
    {
        SwitchState(stateDictionary[stateChange]);
    }
}
