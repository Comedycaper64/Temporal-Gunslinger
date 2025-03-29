using UnityEngine;

public class MaskStateMachine : EnemyRangedStateMachine
{
    [SerializeField]
    private bool reaperMask = false;

    [SerializeField]
    private AudioClip fireSFX;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new BlankState(this));

        if (reaperMask)
        {
            stateDictionary.Add(StateEnum.dead, new EnemyReaperMaskDeadState(this));
        }
        else
        {
            stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
        }
    }

    public void EnableFireProjectile()
    {
        //fireProjectile = true;
        stateDictionary.Remove(StateEnum.active);
        stateDictionary.Add(StateEnum.active, new EnemyMaskShootState(this));
    }

    public AudioClip GetFireSFX()
    {
        return fireSFX;
    }
}
