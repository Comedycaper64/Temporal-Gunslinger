using System.Collections.Generic;
using UnityEngine;

public class EnemyHuskStateMachine : StateMachine
{
    [SerializeField]
    private MentalLink mentalLink;

    [SerializeField]
    private List<GameObject> bodyColliders = new List<GameObject>();

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.active, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        foreach (GameObject collider in bodyColliders)
        {
            collider.SetActive(!toggle);
        }
    }

    public override void SwitchState(State newState)
    {
        mentalLink.LinkSever();
        base.SwitchState(newState);
    }

    public MentalLink GetMentalLink()
    {
        return mentalLink;
    }
}
