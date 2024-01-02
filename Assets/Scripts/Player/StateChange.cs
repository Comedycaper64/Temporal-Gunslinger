using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChange : IRewindableAction
{
    private float timestamp;

    private StateMachine stateMachine;
    private State previousState;

    public static void StateChanged(State previousState, StateMachine stateMachine)
    {
        if (!RewindManager.Instance)
        {
            return;
        }

        StateChange stateChange = new StateChange(previousState, stateMachine);
    }

    public StateChange(State previousState, StateMachine stateMachine)
    {
        this.previousState = previousState;
        this.stateMachine = stateMachine;

        Execute();
    }

    public void Execute()
    {
        RewindManager rewindManager = RewindManager.Instance;

        if (rewindManager.GetTimerActive() && !rewindManager.GetRewindActive())
        {
            rewindManager.AddRewindable(this);
        }
    }

    public float GetTimestamp()
    {
        return timestamp;
    }

    public void SetTimestamp(float timestamp)
    {
        this.timestamp = timestamp;
    }

    public void Undo()
    {
        stateMachine.SwitchState(previousState);
    }
}
