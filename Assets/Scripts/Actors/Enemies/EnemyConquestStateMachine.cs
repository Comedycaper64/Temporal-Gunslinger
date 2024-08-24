using UnityEngine;

public class EnemyConquestStateMachine : EnemyRangedStateMachine
{
    [SerializeField]
    private Animator projectileBulletHolder;

    private readonly int SpawnHash = Animator.StringToHash("Sword Spawn");

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyConquestActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyBossDeadState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        foreach (GameObject collider in bodyColliders)
        {
            collider.SetActive(!toggle);
        }
    }

    public void SetSwordAnimatorPlay()
    {
        projectileBulletHolder.GetComponent<RewindableAnimator>().BeginPlay();
    }

    public void ToggleBulletHolder(bool toggle)
    {
        projectileBulletHolder.gameObject.SetActive(toggle);
    }

    public void SetBulletAnimationTime(float clipTime)
    {
        projectileBulletHolder.Play(SpawnHash, 0, clipTime);
    }
}
