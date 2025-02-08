using System.Collections.Generic;
using UnityEngine;

public class EnemyLibrarianStateMachine : StateMachine
{
    [SerializeField]
    public GameObject shotArrow;

    [SerializeField]
    private float shootTimer;

    [SerializeField]
    protected List<GameObject> bodyColliders = new List<GameObject>();

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyInactiveState(this));
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

    public float GetShootTimer()
    {
        return shootTimer;
    }
}
