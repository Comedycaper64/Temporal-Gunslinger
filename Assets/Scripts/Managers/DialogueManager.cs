using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public struct DialogueUIEventArgs
{
    public DialogueUIEventArgs(ActorSO actorSO, string sentence, Action onTypingFinished)
    {
        this.actorSO = actorSO;
        this.sentence = sentence;
        this.onTypingFinished = onTypingFinished;
    }

    public ActorSO actorSO;
    public string sentence;
    public Action onTypingFinished;
}

public class DialogueManager : MonoBehaviour
{
    private bool bIsSentenceTyping;
    private float crossFadeTime = 0.1f;
    private ActorSO currentActor;
    private int actorIndex;
    private Animator[] actorAnimators;
    private string currentSentence;
    private DialogueCameraDirector dialogueCameraDirector;
    private ActorAnimatorMapper actorAnimatorMapper;
    private Queue<string> currentDialogue;
    private Queue<AnimationClip> currentAnimations;
    private Queue<float> currentAnimationTimes;
    private Queue<CameraMode> currentCameraModes;

    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    public static event Action OnFinishTypingDialogue;
    public static event EventHandler<bool> OnToggleDialogueUI;
    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        dialogueCameraDirector = GetComponent<DialogueCameraDirector>();
        actorAnimatorMapper = GetComponent<ActorAnimatorMapper>();
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        InputManager.Instance.OnShootAction += InputManager_OnShootAction;
        ToggleDialogueUI(true);
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

        AnimatorController actorController = currentActor.GetAnimatorController();

        actorAnimators = actorAnimatorMapper.GetAnimators(actorController);

        actorIndex = dialogueNode.actorNo;
        currentDialogue = new Queue<string>(dialogueNode.dialogue);
        currentAnimations = new Queue<AnimationClip>(dialogueNode.animations);
        currentCameraModes = new Queue<CameraMode>(dialogueNode.cameraModes);
        currentAnimationTimes = new Queue<float>(dialogueNode.animationTime);
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (bIsSentenceTyping)
        {
            OnFinishTypingDialogue?.Invoke();
            return;
        }

        if (!currentDialogue.TryDequeue(out currentSentence))
        {
            TryPlayNextDialogue();
            return;
        }

        if (currentAnimations.TryDequeue(out AnimationClip animation) && (animation != null))
        {
            actorAnimators[actorIndex].CrossFadeInFixedTime(animation.name, crossFadeTime);
        }

        dialogueCameraDirector.ChangeCameraMode(
            currentCameraModes.Dequeue(),
            actorAnimators[actorIndex].transform
        );

        float animationTimer = currentAnimationTimes.Dequeue();

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

    private void StartTypingSentence()
    {
        bIsSentenceTyping = true;

        OnDialogue?.Invoke(
            this,
            new DialogueUIEventArgs(currentActor, currentSentence, FinishTypingSentence)
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

    private void EndDialogue()
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
        ToggleDialogueUI(false);
        dialogueCameraDirector.EndOfDialogueCleanup();
        onDialogueComplete();
    }

    private void InputManager_OnShootAction()
    {
        DisplayNextSentence();
    }
}
