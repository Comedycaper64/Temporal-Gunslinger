using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected float activeAnimationExitTime = 0f;

    [SerializeField]
    protected string activeAnimationName;
    private State currentState;
    protected Dictionary<StateEnum, State> stateDictionary = new Dictionary<StateEnum, State>();

    public Animator stateMachineAnimator;

    [SerializeField]
    private DissolveController dissolveController;

    //REMOVE ON RELEASE
    private void DebugKill()
    {
        SwitchToDeadState();
    }

    public virtual void Awake()
    {
        SetupDictionary();
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
    }

    public virtual void OnDisable()
    {
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
    }

    void Update()
    {
        currentState?.Tick(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.K))
        {
            DebugKill();
        }
    }

    public virtual void SwitchState(State newState)
    {
        if (currentState?.GetType() == newState.GetType())
        {
            return;
        }

        StateChange.StateChanged(currentState, this);
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public virtual void UndoState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public virtual void SwitchToDeadState()
    {
        SwitchState(stateDictionary[StateEnum.dead]);
    }

    protected abstract void SetupDictionary();

    protected State GetCurrentState()
    {
        return currentState;
    }

    public abstract void ToggleInactive(bool toggle);

    public virtual void GameManager_OnGameStateChange(object sender, StateEnum stateChange)
    {
        SwitchState(stateDictionary[stateChange]);
    }

    public void SetRunAnimationExitTime(float newTime)
    {
        activeAnimationExitTime = newTime;
    }

    public string GetActiveAnimationName()
    {
        return activeAnimationName;
    }

    public float GetActiveAnimationExitTime()
    {
        return activeAnimationExitTime;
    }

    public DissolveController GetDissolveController()
    {
        return dissolveController;
    }
}
