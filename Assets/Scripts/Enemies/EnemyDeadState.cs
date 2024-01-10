using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : State
{
    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        //Temp debug death
        stateMachine.transform.position += new Vector3(0, -10, 0);
    }

    public override void Exit()
    {
        stateMachine.transform.position += new Vector3(0, 10, 0);
    }

    public override void Tick(float deltaTime) { }
}
