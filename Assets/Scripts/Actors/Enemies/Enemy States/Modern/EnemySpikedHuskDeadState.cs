using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpikedHuskDeadState : EnemyDeadState
{
    private EnemySpikedHuskStateMachine enemyStateMachine;

    public EnemySpikedHuskDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        //enemiesAlive++;
        animator = stateMachine.stateMachineAnimator;
        dissolveController = stateMachine.GetDissolveController();
        enemyStateMachine = stateMachine as EnemySpikedHuskStateMachine;
    }

    public override void Enter()
    {
        LaunchSpikes();
        base.Enter();
    }

    public override void Exit()
    {
        ResetSpikes();
        base.Exit();
    }

    private void LaunchSpikes()
    {
        foreach (BulletStateMachine spike in enemyStateMachine.spikeProjectiles)
        {
            spike.SwitchToActive();

            //Might break available bullet count but testing
            //Should only be doing this if Spawned Bullet is true
            BulletDeadState.bulletNumber++;
        }
    }

    private void ResetSpikes()
    {
        for (int i = 0; i < enemyStateMachine.spikeProjectiles.Length; i++)
        {
            Bullet bullet = enemyStateMachine.spikeProjectiles[i].GetComponent<Bullet>();
            bullet.ResetBullet(enemyStateMachine.spikePositions[i]);
            BulletDeadState.bulletNumber--;
        }
    }
}
