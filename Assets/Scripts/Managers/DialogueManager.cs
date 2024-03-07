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
    private Animator actorAnimator;
    private string currentSentence;
    private DialogueCameraDirector dialogueCameraDirector;
    private Queue<string> currentDialogue;
    private Queue<AnimationClip> currentAnimations;
    private Queue<CameraMode> currentCameraModes;
    private Dictionary<AnimatorController, Animator> actorAnimatorPair =
        new Dictionary<AnimatorController, Animator>();

    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    public static event Action OnFinishTypingDialogue;
    public static event EventHandler<bool> OnToggleDialogueUI;
    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        dialogueCameraDirector = GetComponent<DialogueCameraDirector>();
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        InputManager.Instance.OnShootAction += InputManager_OnShootAction;
        OnToggleDialogueUI?.Invoke(this, true);
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
        if (!actorAnimatorPair.TryGetValue(actorController, out Animator actorAnimator))
        {
            //Finds Gameobject with the animator controller, adds to dictionary
            Animator[] animators = FindObjectsOfType<Animator>();
            Animator desiredAnimator = animators.First(
                animator => animator.runtimeAnimatorController == actorController
            );
            if (!desiredAnimator)
            {
                Debug.LogError("Animator not found");
                return;
            }
            actorAnimator = desiredAnimator;
            actorAnimatorPair.Add(actorController, actorAnimator);
        }

        this.actorAnimator = actorAnimator;
        currentDialogue = new Queue<string>(dialogueNode.dialogue);
        currentAnimations = new Queue<AnimationClip>(dialogueNode.animations);
        currentCameraModes = new Queue<CameraMode>(dialogueNode.cameraModes);
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
            actorAnimator.CrossFadeInFixedTime(animation.name, crossFadeTime);
        }

        dialogueCameraDirector.ChangeCameraMode(
            currentCameraModes.Dequeue(),
            actorAnimator.transform
        );

        StartTypingSentence();
    }

    private void StartTypingSentence()
    {
        bIsSentenceTyping = true;

        OnDialogue?.Invoke(
            this,
            new DialogueUIEventArgs(currentActor, currentSentence, FinishTypingSentence)
        );
    }

    private void FinishTypingSentence()
    {
        bIsSentenceTyping = false;
    }

    private void EndDialogue()
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
        OnToggleDialogueUI?.Invoke(this, false);
        dialogueCameraDirector.ChangeCameraMode(CameraMode.none, transform);
        onDialogueComplete();
    }

    private void InputManager_OnShootAction()
    {
        DisplayNextSentence();
    }
}
