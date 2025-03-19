using UnityEngine;

public class EnemyReaperStateMachine : EnemyMeleeStateMachine
{
    [SerializeField]
    private RepeatingMaskSpawner maskSpawner;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyReaperIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyReaperActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyBossDeadState(this));
    }

    public void ResetMaskSpawner()
    {
        maskSpawner.ResetSpawner();
    }
}
