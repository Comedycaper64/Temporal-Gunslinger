using UnityEngine;

public class EnemyDeathScytheBAltState : State
{
    private bool quillThrown = false;
    private bool killBoxEnabled = false;
    private float timer = 0f;
    private float animationTimeNormalised = 0f;
    private float throwTime = 0.016f;

    private float attackTime = 0.07f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;
    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Scythe Sweep 2 Alt";

    public EnemyDeathScytheBAltState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        deathSM.transform.position = deathSM.GetScytheBPosition().position;
        deathSM.transform.rotation = deathSM.GetScytheBPosition().rotation;

        if (rewindState.IsRewinding())
        {
            timer = deathSM.GetDurationTime();
            deathSM.GetWeapon().SetTimeOffset(-timer);
            stateMachine.stateMachineAnimator.CrossFade(
                StateAnimHash,
                0f,
                0,
                deathSM.GetAnimationTime()
            );
            quillThrown = true;
        }
        else
        {
            timer = 0f;
            deathSM.GetWeapon().SetTimeOffset(0f);
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
            quillThrown = false;
        }
        deathSM.GetWeapon().ToggleScythe(true);
        deathSM.GetWeapon().OnHit += CounterAttack;
    }

    public override void Exit()
    {
        deathSM.GetWeapon().ToggleScythe(false);
        deathSM.GetWeapon().OnHit -= CounterAttack;
        deathSM.GetSpell().EnableKillBox(false);

        if (!rewindState.IsRewinding())
        {
            deathSM.AddAnimationTime(animationTimeNormalised);
            deathSM.AddDurationTime(timer);
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;

        if (!quillThrown && (timer > throwTime))
        {
            ThrowQuill();
            quillThrown = true;
        }
        else if (quillThrown && (timer <= throwTime))
        {
            quillThrown = false;
        }

        if (!killBoxEnabled && (timer > attackTime))
        {
            deathSM.GetSpell().EnableKillBox(true);
        }
        else if (killBoxEnabled && (timer <= attackTime))
        {
            deathSM.GetSpell().EnableKillBox(false);
        }
    }

    private void ThrowQuill()
    {
        BulletStateMachine quill = deathSM.GetQuill();

        if (!quill)
        {
            return;
        }

        deathSM.SetQuillAtFiringPoint(quill, 2);
        quill.SwitchToActive();

        BulletLockOn bulletLockOn = quill.GetComponent<BulletLockOn>();
        bulletLockOn.LockOnOverride(deathSM.GetRevenantTarget());
    }

    private void CounterAttack()
    {
        deathSM.SwitchState(
            new EnemyDeathTeleportBufferState(deathSM, new EnemyDeathRestingState(deathSM, true))
        );
    }
}
