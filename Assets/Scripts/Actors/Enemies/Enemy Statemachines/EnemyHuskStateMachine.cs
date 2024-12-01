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
        mentalLink.OnLinkFeedback += MentalLink_OnLinkFeedback;
    }

    private void OnDisable()
    {
        mentalLink.OnLinkFeedback -= MentalLink_OnLinkFeedback;
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyHuskIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyHuskIdleState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        foreach (GameObject collider in bodyColliders)
        {
            collider.SetActive(!toggle);
        }
    }

    public override void SwitchToDeadState()
    {
        mentalLink.LinkSever();
        base.SwitchToDeadState();
    }

    public MentalLink GetMentalLink()
    {
        return mentalLink;
    }

    private void MentalLink_OnLinkFeedback()
    {
        base.SwitchToDeadState();
    }
}
