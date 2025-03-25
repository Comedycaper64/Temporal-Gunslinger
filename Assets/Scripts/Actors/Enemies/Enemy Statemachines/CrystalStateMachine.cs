using UnityEngine;

public class CrystalStateMachine : StateMachine
{
    [SerializeField]
    protected GameObject crystalCollider;

    public override void ToggleInactive(bool toggle)
    {
        crystalCollider.SetActive(!toggle);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new BlankState(this));
        stateDictionary.Add(StateEnum.idle, new BlankState(this));
        stateDictionary.Add(StateEnum.active, new BlankState(this));
        stateDictionary.Add(StateEnum.dead, new CrystalDeadState(this));
    }
}
