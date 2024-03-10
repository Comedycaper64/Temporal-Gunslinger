using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueUI : MonoBehaviour
{
    private bool bIsDialogueActive;
    private bool bDialogueActiveChanged;
    private bool isTyping = false;
    private float timeBetweenLetterTyping = 0.05f;
    private float textBoxFadeSpeed = 5f;
    private string typingSentence;
    private Coroutine typingCoroutine;
    private Action onTypingFinished;

    [SerializeField]
    private Image textBox;

    [SerializeField]
    private TextMeshProUGUI actorNameText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        DialogueManager.OnToggleDialogueUI += DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue += DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue += DialogueManager_OnFinishTypingDialogue;

        ClearDialogueText();
        SetActorName("");
        textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 0f);
    }

    private void OnDisable()
    {
        DialogueManager.OnToggleDialogueUI -= DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue -= DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue -= DialogueManager_OnFinishTypingDialogue;
    }

    private void Update()
    {
        if (bDialogueActiveChanged)
        {
            FadeTextBox();
        }
    }

    private void FadeTextBox()
    {
        if (bIsDialogueActive)
        {
            textBox.color = new Color(
                textBox.color.r,
                textBox.color.g,
                textBox.color.b,
                textBox.color.a + Time.deltaTime * textBoxFadeSpeed
            );
            if (textBox.color.a > 0.99f)
            {
                bDialogueActiveChanged = false;
            }
        }
        else
        {
            textBox.color = new Color(
                textBox.color.r,
                textBox.color.g,
                textBox.color.b,
                textBox.color.a - Time.deltaTime * textBoxFadeSpeed
            );
            if (textBox.color.a < 0.01f)
            {
                bDialogueActiveChanged = false;
            }
        }
    }

    private void SetActorName(string actorName)
    {
        actorNameText.text = actorName;
    }

    private void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    private void ToggleDialogueActive(bool toggle)
    {
        bIsDialogueActive = toggle;
        bDialogueActiveChanged = true;
    }

    private IEnumerator TypeSentence(DialogueUIEventArgs dialogueUIEventArgs)
    {
        ActorSO actorSO = dialogueUIEventArgs.actorSO;
        typingSentence = dialogueUIEventArgs.sentence;
        onTypingFinished = dialogueUIEventArgs.onTypingFinished;

        SetActorName(actorSO.GetActorName());
        ClearDialogueText();
        AudioClip[] actorClips = actorSO.GetDialogueNoises();
        int clipsLength = actorClips.Length;

        isTyping = true;
        foreach (char letter in typingSentence.ToCharArray())
        {
            dialogueText.text += letter;

            AudioClip textSound = actorClips[Random.Range(0, clipsLength - 1)];
            //get random dialogue sound from actor and play it in audio source

            yield return new WaitForSeconds(timeBetweenLetterTyping);
        }
        isTyping = false;
        onTypingFinished();
    }

    private void DialogueManager_OnFinishTypingDialogue()
    {
        if (!isTyping)
        {
            return;
        }

        StopCoroutine(typingCoroutine);
        dialogueText.text = typingSentence;
        onTypingFinished();
    }

    private void DialogueManager_OnDialogue(object sender, DialogueUIEventArgs dialogueArgs)
    {
        typingCoroutine = StartCoroutine(TypeSentence(dialogueArgs));
    }

    private void DialogueManager_OnToggleDialogueUI(object sender, bool e)
    {
        if (e == bIsDialogueActive)
        {
            return;
        }

        ClearDialogueText();
        SetActorName("");
        ToggleDialogueActive(e);
    }
}
