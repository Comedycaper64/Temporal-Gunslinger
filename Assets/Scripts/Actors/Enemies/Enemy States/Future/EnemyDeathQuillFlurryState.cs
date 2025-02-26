using UnityEngine;

public class EnemyDeathQuillFlurryState : State
{
    private bool quillThrown = false;
    private float timer = 0f;
    private float throwTime = 0.02f;
    private float stateTime = 0.06f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Quill Flurry";

    public EnemyDeathQuillFlurryState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        Debug.Log(" Quill Pattern ");

        deathSM.GetDissolveController().StartDissolve(deathSM.etherealDissolveValue);

        if (rewindState.IsRewinding())
        {
            timer = stateTime;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0, 1f);
            quillThrown = true;
        }
        else
        {
            timer = 0f;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
            quillThrown = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        if (timer > stateTime)
        {
            deathSM.SwitchState(new EnemyDeathRestingState(deathSM, false));
        }
    }

    public override void Exit() { }
}
