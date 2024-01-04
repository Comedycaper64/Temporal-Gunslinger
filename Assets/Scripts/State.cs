using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

    public virtual string GetStateName()
    {
        return "State";
    }
}
