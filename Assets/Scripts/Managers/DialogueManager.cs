using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogueUIEventArgs
{
    public DialogueUIEventArgs(
        ActorSO actorSO,
        string sentence,
        bool playDialogueNoises,
        Action onTypingFinished
    )
    {
        this.actorSO = actorSO;
        this.sentence = sentence;
        this.playDialogueNoises = playDialogueNoises;
        this.onTypingFinished = onTypingFinished;
    }

    public ActorSO actorSO;
    public string sentence;
    public bool playDialogueNoises;
    public Action onTypingFinished;
}

public struct DialogueChoiceUIEventArgs
{
    public DialogueChoiceUIEventArgs(
        DialogueChoiceSO dialogueChoice,
        EventHandler<DialogueChoice> onDialogueChosen
    )
    {
        this.dialogueChoice = dialogueChoice;
        this.onDialogueChosen = onDialogueChosen;
    }

    public DialogueChoiceSO dialogueChoice;
    public EventHandler<DialogueChoice> onDialogueChosen;
}

public class DialogueManager : MonoBehaviour
{
    private bool bIsSentenceTyping;
    private bool bLoopToChoice;
    private bool bAutoPlay = false;
    private float crossFadeTime = 0.1f;
    private Coroutine autoPlayCoroutine;
    private Coroutine animationCoroutine;
    private ActorSO currentActor;
    private DialogueChoiceSO currentChoice;
    private int actorIndex;
    private Animator[] actorAnimators;
    private string currentSentence;
    private AudioSource dialogueAudioSource;
    private DialogueCameraDirector dialogueCameraDirector;
    private ActorAnimatorMapper actorAnimatorMapper;
    private Queue<string> currentDialogue;
    private Queue<AnimationClip> currentAnimations;
    private Queue<float> currentAnimationCrossFadeTimes;
    private Queue<float> currentAnimationTimes;
    private Queue<CameraMode> currentCameraModes;
    private Queue<AudioClip> currentVoiceClips;
    private bool disableCameraOnEnd;

    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    public static event Action OnFinishTypingDialogue;
    public static event EventHandler<bool> OnToggleDialogueUI;
    public static event EventHandler<DialogueChoiceUIEventArgs> OnDisplayChoices;

    //public static event EventHandler<Sprite[]> OnChangeSprite;
    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        dialogueCameraDirector = GetComponent<DialogueCameraDirector>();
        actorAnimatorMapper = GetComponent<ActorAnimatorMapper>();
        dialogueAudioSource = GetComponent<AudioSource>();

        DialogueAutoPlayUI.OnAutoPlayToggle += ToggleAutoPlay;
    }

    private void OnDisable()
    {
        DialogueAutoPlayUI.OnAutoPlayToggle -= ToggleAutoPlay;
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        InputManager.Instance.OnShootAction += InputManager_OnShootAction;

        ToggleDialogueUI(true);
        TryPlayNextDialogue();
    }

    public void SkipCurrentDialogue()
    {
        DialogueSkipCleanup();

        while (currentAnimations.Count > 0)
        {
            TryPlayAnimation();
        }

        GoThroughDialogueAnimations();

        EndDialogue(true);
    }

    public void SkipCurrentChoice()
    {
        DialogueSkipCleanup();

        EndDialogue(true);
    }

    public void SkipDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        DialogueSkipCleanup();

        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());

        GoThroughDialogueAnimations();

        onDialogueComplete();
    }

    private void DialogueSkipCleanup()
    {
        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        dialogueCameraDirector.EndOfDialogueCleanup();

        dialogueAudioSource.Stop();
    }

    private void GoThroughDialogueAnimations()
    {
        foreach (Dialogue dialogue in dialogues)
        {
            currentActor = dialogue.actor;
            RuntimeAnimatorController actorController = currentActor.GetAnimatorController();
            actorAnimators = actorAnimatorMapper.GetAnimators(actorController);

            actorIndex = dialogue.actorNo;
            currentAnimations = new Queue<AnimationClip>(dialogue.animations);
            //currentAnimationTimes = new Queue<float>(dialogue.animationTime);

            if (dialogue.animationCrossFadeTime != null)
            {
                currentAnimationCrossFadeTimes = new Queue<float>(dialogue.animationCrossFadeTime);
            }
            else
            {
                currentAnimationCrossFadeTimes = new Queue<float>();
            }

            while (currentAnimations.Count > 0)
            {
                TryPlayAnimation();
            }
        }
    }

    public void DisplayChoices(DialogueChoiceSO dialogueChoiceSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        currentChoice = dialogueChoiceSO;
        ToggleDialogueUI(true);
        DialogueChoiceUIEventArgs choiceUIEventArgs = new DialogueChoiceUIEventArgs(
            dialogueChoiceSO,
            PlayChoiceDialogue
        );
        OnDisplayChoices?.Invoke(this, choiceUIEventArgs);
    }

    private void PlayChoiceDialogue(object sender, DialogueChoice dialogueChoice)
    {
        if (dialogueChoice.loopBackToChoice)
        {
            bLoopToChoice = true;
        }

        dialogues = new Queue<Dialogue>(
            currentChoice.GetDialogueAnswers()[dialogueChoice.correspondingDialogue].dialogueAnswers
        );
        InputManager.Instance.OnShootAction += InputManager_OnShootAction;

        TryPlayNextDialogue();
    }

    private void TryPlayNextDialogue()
    {
        if (!dialogues.TryDequeue(out Dialogue dialogueNode))
        {
            EndDialogue();
            return;
        }
        currentActor = dialogueNode.actor;

        RuntimeAnimatorController actorController = currentActor.GetAnimatorController();

        //Sprite[] actorSprites = currentActor.GetActorSprites();

        //OnChangeSprite?.Invoke(this, actorSprites);

        actorAnimators = actorAnimatorMapper.GetAnimators(actorController);

        actorIndex = dialogueNode.actorNo;
        currentDialogue = new Queue<string>(dialogueNode.dialogue);
        currentAnimations = new Queue<AnimationClip>(dialogueNode.animations);
        currentCameraModes = new Queue<CameraMode>(dialogueNode.cameraModes);
        currentAnimationTimes = new Queue<float>(dialogueNode.animationTime);
        dialogueAudioSource.outputAudioMixerGroup = currentActor?.GetAudioMixer();

        if (dialogueNode.animationCrossFadeTime != null)
        {
            currentAnimationCrossFadeTimes = new Queue<float>(dialogueNode.animationCrossFadeTime);
        }
        else
        {
            currentAnimationCrossFadeTimes = new Queue<float>();
        }

        if (dialogueNode.voiceClip != null)
        {
            currentVoiceClips = new Queue<AudioClip>(dialogueNode.voiceClip);
        }
        else
        {
            currentVoiceClips = new Queue<AudioClip>();
        }

        disableCameraOnEnd = dialogueNode.disableCameraOnEnd;
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        //Debug.Log("Go, go go!");

        if (bIsSentenceTyping)
        {
            //Debug.Log("We're typin here!");
            OnFinishTypingDialogue?.Invoke();
            return;
        }

        //Debug.Log("Next sentence");

        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }

        if (!currentDialogue.TryDequeue(out currentSentence))
        {
            TryPlayNextDialogue();
            return;
        }

        TryPlayAnimation();

        TryChangeCameraMode();

        TryPlayVoiceClip();

        float animationTimer = TrySetAnimationTimer();

        if (animationTimer > 0f)
        {
            animationCoroutine = StartCoroutine(AnimationPause(animationTimer));
        }

        if (currentSentence == "")
        {
            ToggleDialogueUI(false);

            if (animationTimer == 0f)
            {
                animationCoroutine = StartCoroutine(AnimationPause(animationTimer));
            }
        }
        else
        {
            ToggleDialogueUI(true);
            StartTypingSentence();
        }
    }

    private void TryChangeCameraMode()
    {
        if (currentCameraModes.TryDequeue(out CameraMode cameraMode))
        {
            if (actorAnimators.Length == 0)
            {
                return;
            }

            dialogueCameraDirector.ChangeCameraMode(
                cameraMode,
                actorAnimators[actorIndex].transform
            );
        }
    }

    private void TryPlayAnimation()
    {
        if (currentAnimations.TryDequeue(out AnimationClip animation) && (animation != null))
        {
            if (actorAnimators.Length == 0)
            {
                return;
            }

            float crossFade = crossFadeTime;

            if (currentAnimationCrossFadeTimes.TryDequeue(out float fadeTime))
            {
                crossFade = fadeTime;
            }

            actorAnimators[actorIndex].CrossFadeInFixedTime(animation.name, crossFade);
        }
    }

    private void TryPlayVoiceClip()
    {
        if (currentVoiceClips.TryDequeue(out AudioClip voiceClip) && (voiceClip != null))
        {
            dialogueAudioSource.clip = voiceClip;
            dialogueAudioSource.volume =
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetVoiceVolume();
            dialogueAudioSource.Play();

            if (bAutoPlay)
            {
                autoPlayCoroutine = StartCoroutine(
                    DialogueAutoPlayTimer(
                        voiceClip.length * (1 / currentActor.GetAudioMixerPitchMod())
                    )
                );
            }
        }
    }

    private float TrySetAnimationTimer()
    {
        if (currentAnimationTimes.TryDequeue(out float animationTime))
        {
            return animationTime;
        }
        else
        {
            return 0f;
        }
    }

    private IEnumerator DialogueAutoPlayTimer(float dialogueTime)
    {
        yield return new WaitForSeconds(dialogueTime);
        DisplayNextSentence();
    }

    private void StartTypingSentence()
    {
        bIsSentenceTyping = true;

        bool playDialogueNoises = !dialogueAudioSource.isPlaying;

        OnDialogue?.Invoke(
            this,
            new DialogueUIEventArgs(
                currentActor,
                currentSentence,
                playDialogueNoises,
                FinishTypingSentence
            )
        );
    }

    private IEnumerator AnimationPause(float pauseTime)
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;

        yield return new WaitForSeconds(pauseTime);

        InputManager.Instance.OnShootAction += InputManager_OnShootAction;

        animationCoroutine = null;

        DisplayNextSentence();
    }

    private void FinishTypingSentence()
    {
        bIsSentenceTyping = false;
    }

    private void ToggleDialogueUI(bool toggle)
    {
        OnToggleDialogueUI?.Invoke(this, toggle);
    }

    private void ResetChoices()
    {
        DialogueChoiceUIEventArgs blankChoiceUIEventArgs = new DialogueChoiceUIEventArgs(
            null,
            null
        );
        OnDisplayChoices?.Invoke(this, blankChoiceUIEventArgs);
    }

    private void EndDialogue(bool skipping = false)
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;

        dialogueAudioSource.Stop();

        ToggleDialogueUI(false);
        if (disableCameraOnEnd)
        {
            dialogueCameraDirector.EndOfDialogueCleanup();
        }

        if (bLoopToChoice)
        {
            bLoopToChoice = false;
            DisplayChoices(currentChoice, onDialogueComplete);
        }
        else
        {
            //reset colour on choices
            ResetChoices();

            if (!skipping)
            {
                onDialogueComplete();
            }
            else
            {
                onDialogueComplete = null;
            }
        }
    }

    // public void SetDialogueCamera(CameraMode cameraMode, ActorSO cameraTarget)
    // {
    //     if (cameraMode == CameraMode.none)
    //     {
    //         return;
    //     }

    //     dialogueCameraDirector.ChangeCameraMode(
    //         cameraMode,
    //         actorAnimatorMapper.GetAnimators(cameraTarget.GetAnimatorController())[0].transform
    //     );
    // }

    private void InputManager_OnShootAction()
    {
        DisplayNextSentence();
    }

    private void ToggleAutoPlay(object sender, bool e)
    {
        bAutoPlay = e;

        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }
    }
}
