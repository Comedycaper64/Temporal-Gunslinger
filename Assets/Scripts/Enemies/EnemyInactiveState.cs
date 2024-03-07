using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInactiveState : State
{
    public EnemyInactiveState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter() { }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
