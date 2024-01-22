using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    private bool bIsSentenceTyping;
    private float timeBetweenLetterTyping = 0.05f;
    private ActorSO currentActor;
    private Animator actorAnimator;
    private string currentSentence;
    private DialogueCameraDirector dialogueCameraDirector;
    private Queue<string> currentDialogue;
    private Queue<AnimationClip> currentAnimations;
    private Queue<CameraMode> currentCameraModes;
    private Dictionary<AnimatorController, Animator> actorAnimatorPair =
        new Dictionary<AnimatorController, Animator>();
    private Coroutine typingCoroutine;

    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one DialogueManager! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dialogueCameraDirector = GetComponent<DialogueCameraDirector>();
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
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

        //Set name text to be actor name
        AnimatorController actorController = currentActor.GetAnimatorController();
        if (!actorAnimatorPair.TryGetValue(actorController, out Animator actorAnimator))
        {
            //Find Gameobject with the animator controller, add to dictionary
            Animator[] animators = FindObjectsOfType<Animator>();
            Animator desiredAnimator = animators.Single(
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
            FinishTypingSentence();
            return;
        }

        if (!currentDialogue.TryDequeue(out currentSentence))
        {
            TryPlayNextDialogue();
        }

        actorAnimator.Play(currentAnimations.Dequeue().GetHashCode());

        dialogueCameraDirector.ChangeCameraMode(
            currentCameraModes.Dequeue(),
            actorAnimator.transform
        );

        typingCoroutine = StartCoroutine(TypeSentence());
    }

    private IEnumerator TypeSentence()
    {
        bIsSentenceTyping = true;
        //Clear dialogue Ui text
        AudioClip[] actorClips = currentActor.GetDialogueNoises();
        int clipsLength = actorClips.Length;
        foreach (char letter in currentSentence.ToCharArray())
        {
            //dialogueText.text += letter;

            AudioClip textSound = actorClips[Random.Range(0, clipsLength - 1)];
            //get random dialogue sound from actor and play it in audio source

            yield return new WaitForSeconds(timeBetweenLetterTyping);
        }
        bIsSentenceTyping = false;
    }

    private void FinishTypingSentence()
    {
        StopCoroutine(typingCoroutine);
        //Set dialogue UI text to be current sentence
        bIsSentenceTyping = false;
    }

    private void EndDialogue()
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
        //Withdraw dialogue UI
        onDialogueComplete();
    }

    private void InputManager_OnShootAction()
    {
        DisplayNextSentence();
    }
}
