using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathStateMachine : StateMachine
{
    public float etherealDissolveValue = 0.25f;
    private int flowIndex = 0;
    private List<State> stateFlow = new List<State>();
    private Stack<float> stateAnimationTimes = new Stack<float>();
    private Stack<float> stateDurationTimes = new Stack<float>();

    [SerializeField]
    private Transform restPosition;

    [SerializeField]
    private Transform scytheAPosition;

    [SerializeField]
    private Transform scytheBPosition;

    [SerializeField]
    private DeathScythe deathScythe;

    [SerializeField]
    private DeathHealth health;

    [SerializeField]
    private RewindState rewindState;

    public override void ToggleInactive(bool toggle) { }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyDeathIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyDeathRestingState(this, false));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));

        stateFlow.Add(new EnemyDeathScytheAAltState(this));
        stateFlow.Add(new EnemyDeathScytheBAltState(this));
        stateFlow.Add(new EnemyDeathQuillFlurryState(this));
        stateFlow.Add(new EnemyDeathHeavyCastState(this));

        stateFlow.Add(new EnemyDeathDeadzonesState(this));
        stateFlow.Add(new EnemyDeathScytheBAltState(this));
        stateFlow.Add(new EnemyDeathQuillFlurryState(this));
        stateFlow.Add(new EnemyDeathScytheAState(this));
        stateFlow.Add(new EnemyDeathHeavyCastState(this));

        stateFlow.Add(new EnemyDeathDeadzonesState(this));
        stateFlow.Add(new EnemyDeathQuillFlurryState(this));
        stateFlow.Add(new EnemyDeathScytheAState(this));
        stateFlow.Add(new EnemyDeathScytheBState(this));
        stateFlow.Add(new EnemyDeathHeavyCastState(this));
    }

    public void ResetFlow()
    {
        flowIndex = 0;
        stateAnimationTimes = new Stack<float>();
        stateDurationTimes = new Stack<float>();
    }

    public void IncrementFlow()
    {
        flowIndex++;
    }

    public void DecrementFlow()
    {
        flowIndex--;
    }

    public void AddAnimationTime(float time)
    {
        stateAnimationTimes.Push(time);
    }

    public void AddDurationTime(float time)
    {
        stateDurationTimes.Push(time);
    }

    public float GetAnimationTime()
    {
        return stateAnimationTimes.Pop();
    }

    public float GetDurationTime()
    {
        return stateDurationTimes.Pop();
    }

    public Transform GetRestPosition()
    {
        return restPosition;
    }

    public Transform GetScytheAPosition()
    {
        return scytheAPosition;
    }

    public Transform GetScytheBPosition()
    {
        return scytheBPosition;
    }

    public State GetNextState()
    {
        return stateFlow[flowIndex];
    }

    public DeathScythe GetWeapon()
    {
        return deathScythe;
    }

    public DeathHealth GetHealth()
    {
        return health;
    }

    public RewindState GetRewindState()
    {
        return rewindState;
    }
}
