using System;
using UnityEngine;

public class EnemyDeathHeavyCastInterruptState : State
{
    private float timer = 0f;
    private float stateTime = 0.015f;
    private AudioSource lineAudioSource;
    private RewindState rewindState;
    private EnemyDeathStateMachine deathSM;

    private readonly int StateAnimHash;
    private readonly string StateAnimName = "Death Heavy Cast Interrupt";
    public static EventHandler<string> OnDeathInterrupted;

    public EnemyDeathHeavyCastInterruptState(StateMachine stateMachine)
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
        }
        else
        {
            timer = 0f;
            stateMachine.stateMachineAnimator.CrossFade(StateAnimHash, 0f, 0);

            if (deathSM.GetFlowIndex() < 6)
            {
                lineAudioSource = deathSM.GetDeathAudioSource();
                lineAudioSource.clip = deathSM.GetDeathLines()[0];
                lineAudioSource.Play();

                OnDeathInterrupted?.Invoke(this, deathSM.GetDeathLineText()[0]);
            }
            else
            {
                lineAudioSource = deathSM.GetDeathAudioSource();
                lineAudioSource.clip = deathSM.GetDeathLines()[1];
                lineAudioSource.Play();

                OnDeathInterrupted?.Invoke(this, deathSM.GetDeathLineText()[1]);
            }
        }
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime * rewindState.GetScaledSpeed();

        if (timer > stateTime)
        {
            //deathSM.SwitchState(new EnemyDeathRestingState(deathSM, false));
            deathSM.SwitchState(
                new EnemyDeathTeleportBufferState(
                    deathSM,
                    new EnemyDeathRestingState(deathSM, false)
                )
            );
        }
    }

    public override void Exit()
    {
        if (lineAudioSource)
        {
            lineAudioSource.Stop();
        }
    }
}
