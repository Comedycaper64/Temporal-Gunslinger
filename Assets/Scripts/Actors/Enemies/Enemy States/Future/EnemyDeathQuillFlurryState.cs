using UnityEngine;

public class EnemyDeathQuillFlurryState : State
{
    private bool quillThrown = false;
    private int quillThrowNum = 3;
    private float timer = 0f;
    private float throwTime = 0.014f;
    private float stateTime = 0.024f;
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

        if (!quillThrown && (timer > throwTime))
        {
            ThrowQuills();
            quillThrown = true;
        }
        else if (quillThrown && (timer <= throwTime))
        {
            quillThrown = false;
        }

        if (timer > stateTime)
        {
            deathSM.SwitchState(new EnemyDeathRestingState(deathSM, false));
        }
    }

    private void ThrowQuills()
    {
        for (int i = 0; i < quillThrowNum; i++)
        {
            BulletStateMachine quill = deathSM.GetQuill();

            if (!quill)
            {
                continue;
            }

            deathSM.SetQuillAtFiringPoint(quill, i + 3);
            quill.SwitchToActive();

            BulletLockOn bulletLockOn = quill.GetComponent<BulletLockOn>();
            bulletLockOn.LockOnOverride(deathSM.GetRevenantTarget());
        }
    }

    public override void Exit() { }
}
