using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChange : RewindableAction
{
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

    public override void Execute()
    {
        RewindManager rewindManager = RewindManager.Instance;

        if (rewindManager.GetTimerActive() && !rewindManager.GetRewindActive())
        {
            rewindManager.AddRewindable(this);
        }
    }

    public override void Undo()
    {
        stateMachine.SwitchState(previousState);
    }
}
