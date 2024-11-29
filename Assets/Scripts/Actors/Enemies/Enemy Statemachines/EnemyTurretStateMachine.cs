using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretStateMachine : StateMachine
{
    [SerializeField]
    protected List<GameObject> bodyColliders = new List<GameObject>();

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyTurretIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyTurretActiveState(this));
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
