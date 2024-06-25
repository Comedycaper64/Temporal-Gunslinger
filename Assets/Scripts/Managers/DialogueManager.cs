using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool bAutoPlay = true;
    private float crossFadeTime = 0.1f;
    private Coroutine autoPlayCoroutine;
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
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        InputManager.Instance.OnShootAction += InputManager_OnShootAction;
        ToggleDialogueUI(true);
        TryPlayNextDialogue();
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
        if (bIsSentenceTyping)
        {
            OnFinishTypingDialogue?.Invoke();
            return;
        }

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

        if (currentSentence == "")
        {
            ToggleDialogueUI(false);
            StartCoroutine(AnimationPause(animationTimer));
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

            actorAnimators[actorIndex].CrossFadeInFixedTime(animation.name, crossFadeTime);
        }
    }

    private void TryPlayVoiceClip()
    {
        if (currentVoiceClips.TryDequeue(out AudioClip voiceClip) && (voiceClip != null))
        {
            dialogueAudioSource.clip = voiceClip;
            dialogueAudioSource.Play();

            if (bAutoPlay)
            {
                autoPlayCoroutine = StartCoroutine(DialogueAutoPlayTimer(voiceClip.length));
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
        bIsSentenceTyping = true;
        yield return new WaitForSeconds(pauseTime);
        bIsSentenceTyping = false;
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

    private void EndDialogue()
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
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
            onDialogueComplete();
        }
    }

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
