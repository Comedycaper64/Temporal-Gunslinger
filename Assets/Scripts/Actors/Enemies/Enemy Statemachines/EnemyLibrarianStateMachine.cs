using System.Collections.Generic;
using UnityEngine;

public class EnemyLibrarianStateMachine : EnemyRangedStateMachine
{
    [SerializeField]
    public GameObject shotArrow;

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
}
