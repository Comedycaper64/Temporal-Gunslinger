using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    protected Dictionary<StateEnum, State> stateDictionary = new Dictionary<StateEnum, State>();

    public virtual void Awake()
    {
        SetupDictionary();
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
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

    public virtual void SwitchToDeadState()
    {
        SwitchState(stateDictionary[StateEnum.dead]);
    }

    protected abstract void SetupDictionary();

    private void GameManager_OnGameStateChange(object sender, StateEnum stateChange)
    {
        SwitchState(stateDictionary[stateChange]);
    }
}
