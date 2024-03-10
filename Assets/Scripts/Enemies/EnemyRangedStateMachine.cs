using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedStateMachine : StateMachine
{
    [SerializeField]
    private float shootTimer;

    [SerializeField]
    private BulletStateMachine projectile;

    [SerializeField]
    private List<GameObject> bodyColliders = new List<GameObject>();

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyRangedActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleDie(bool toggle)
    {
        foreach (GameObject collider in bodyColliders)
        {
            collider.SetActive(!toggle);
        }
    }

    public float GetShootTimer()
    {
        return shootTimer;
    }

    public BulletStateMachine GetBulletStateMachine()
    {
        return projectile;
    }
}
