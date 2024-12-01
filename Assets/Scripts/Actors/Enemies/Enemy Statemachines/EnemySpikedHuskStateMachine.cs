using UnityEngine;

public class EnemySpikedHuskStateMachine : EnemyHuskStateMachine
{
    public BulletStateMachine[] spikeProjectiles;
    public Transform[] spikePositions;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyHuskIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyHuskIdleState(this));
        stateDictionary.Add(StateEnum.dead, new EnemySpikedHuskDeadState(this));
    }
}
