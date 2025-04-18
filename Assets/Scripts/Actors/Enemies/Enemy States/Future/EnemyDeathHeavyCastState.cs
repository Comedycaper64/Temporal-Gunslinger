using UnityEngine;

public class EnemyDeathHeavyCastState : State
{
    private bool killRevenant = false;
    private float timer = 0f;
    private float animationTimeNormalised = 0f;
    private float killTime = 0.0925f;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;
    private FingerOfDeath deathSpell;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Heavy Cast";

    public EnemyDeathHeavyCastState(StateMachine stateMachine)
        : base(stateMachine)
    {
        deathSM = stateMachine as EnemyDeathStateMachine;
        rewindState = deathSM.GetRewindState();
        deathSpell = deathSM.GetSpell();
        StateAnimHash = Animator.StringToHash(StateAnimName);
    }

    public override void Enter()
    {
        deathSM.GetDissolveController().StartDissolve(0f);
        deathSM.ToggleShield(true);
        deathSM.ToggleWeakPoint(true);

        deathSM.GetHealth().OnDamageTaken += InterruptCast;

        if (rewindState.IsRewinding())
        {
            timer = deathSM.GetDurationTime();
            deathSM.GetSpell().SetTimeOffset(-timer);
            stateMachine.stateMachineAnimator.CrossFade(
                StateAnimHash,
                0f,
                0,
                deathSM.GetAnimationTime()
            );
        }
        else
        {
            timer = 0f;
            deathSM.GetSpell().SetTimeOffset(0f);
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);
        }
        deathSM.GetSpell().ToggleAttack(true);
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();
        animationTimeNormalised = stateMachine.stateMachineAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime;

        float timerLerp = timer / killTime;

        deathSpell.UpdateAttackVisual(timerLerp);

        if (!killRevenant && (timer > killTime))
        {
            deathSpell.EnableKillBox(true);
            killRevenant = true;
        }
        else if (killRevenant && (timer <= killTime))
        {
            deathSpell.EnableKillBox(false);
            killRevenant = false;
        }
    }

    public override void Exit()
    {
        deathSM.ToggleShield(false);
        deathSM.ToggleWeakPoint(false);
        deathSpell.DisableAttackVisual();
        deathSM.GetSpell().ToggleAttack(false);

        deathSM.GetHealth().OnDamageTaken -= InterruptCast;

        if (!rewindState.IsRewinding())
        {
            deathSM.AddAnimationTime(animationTimeNormalised);
            deathSM.AddDurationTime(timer);
        }
    }

    private void InterruptCast()
    {
        if (deathSM.GetIsOutOfMoves())
        {
            deathSM.SwitchToDeadState();
        }
        else
        {
            deathSM.SwitchState(new EnemyDeathHeavyCastInterruptState(deathSM));
        }
    }

    public override string GetStateName()
    {
        return "Heavy Cast State";
    }
}
