using System;
using UnityEngine;

public class EnemyLibrarianStateMachine : EnemyRangedStateMachine, ISetAttacker
{
    [SerializeField]
    public GameObject shotArrow;

    [SerializeField]
    private Renderer sigilRenderer;

    public event EventHandler<bool> OnAttackToggled;
    public event EventHandler<float> OnTimeOffset;

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyRangedIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyLibrarianActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        foreach (GameObject collider in bodyColliders)
        {
            collider.SetActive(!toggle);
        }
    }

    public void ToggleAttack(bool toggle)
    {
        OnAttackToggled?.Invoke(this, toggle);
    }

    public Material GetSigil()
    {
        return sigilRenderer.material;
    }

    public void SetTimeOffset(float offset)
    {
        OnTimeOffset?.Invoke(this, offset);
    }
}
