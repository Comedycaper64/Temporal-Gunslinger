using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretActiveState : State
{
    private RepeatingBulletSpawner turretBulletSpawner;

    public EnemyTurretActiveState(StateMachine stateMachine)
        : base(stateMachine)
    {
        turretBulletSpawner = stateMachine.GetComponent<RepeatingBulletSpawner>();
    }

    public override void Enter()
    {
        turretBulletSpawner.ResetSpawner();
        turretBulletSpawner.ToggleMovement(true);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit()
    {
        turretBulletSpawner.ResetSpawner();
        turretBulletSpawner.ToggleMovement(false);
    }
}
