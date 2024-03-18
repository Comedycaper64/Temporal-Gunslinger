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
    private int spriteArrayIndex = 0;
    private float timeBetweenLetterTyping = 0.05f;
    private float spriteChangeTimer = 0f;

    [SerializeField]
    private float spriteChangeTime = 0.5f;
    private float textBoxFadeSpeed = 2.5f;
    private string typingSentence;
    private Coroutine typingCoroutine;
    private Action onTypingFinished;

    private CanvasGroup dialogueCanvasGroup;

    [SerializeField]
    private Image dialogueFaceSprite;

    private Sprite[] currentSpriteSet = new Sprite[0];

    [SerializeField]
    private TextMeshProUGUI actorNameText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        DialogueManager.OnToggleDialogueUI += DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue += DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue += DialogueManager_OnFinishTypingDialogue;
        //DialogueManager.OnChangeSprite += DialogueManager_OnChangeSprite;

        ClearDialogueText();
        SetActorName("");
        //textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 0f);
        dialogueCanvasGroup = GetComponent<CanvasGroup>();
        dialogueCanvasGroup.alpha = 0f;
    }

    private void OnDisable()
    {
        DialogueManager.OnToggleDialogueUI -= DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue -= DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue -= DialogueManager_OnFinishTypingDialogue;
        //DialogueManager.OnChangeSprite -= DialogueManager_OnChangeSprite;
    }

    private void Update()
    {
        if (currentSpriteSet.Length > 0)
        {
            spriteChangeTimer += Time.deltaTime;

            if (spriteChangeTimer > spriteChangeTime)
            {
                spriteChangeTimer = 0f;
                spriteArrayIndex++;
                if (spriteArrayIndex >= currentSpriteSet.Length)
                {
                    spriteArrayIndex = 0;
                }
                dialogueFaceSprite.sprite = currentSpriteSet[spriteArrayIndex];
            }
        }

        if (bDialogueActiveChanged)
        {
            FadeUI();
        }
    }

    private void FadeUI()
    {
        if (bIsDialogueActive)
        {
            dialogueCanvasGroup.alpha += textBoxFadeSpeed * Time.deltaTime;

            if (dialogueCanvasGroup.alpha >= 1f)
            {
                bDialogueActiveChanged = false;
            }
        }
        else
        {
            dialogueCanvasGroup.alpha -= textBoxFadeSpeed * Time.deltaTime;

            if (dialogueCanvasGroup.alpha <= 0f)
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
        Sprite[] spriteSet = actorSO.GetActorSprites();

        if ((spriteSet == null) || (spriteSet.Length == 0))
        {
            currentSpriteSet = new Sprite[0];
        }
        else
        {
            currentSpriteSet = spriteSet;
            dialogueFaceSprite.sprite = currentSpriteSet[0];
        }

        if (currentSpriteSet.Length <= 0)
        {
            dialogueFaceSprite.gameObject.SetActive(false);
        }
        else
        {
            dialogueFaceSprite.gameObject.SetActive(true);
        }

        spriteChangeTime = actorSO.GetSpriteSwitchTime();

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

    // private void DialogueManager_OnChangeSprite(object sender, Sprite[] e)
    // {
    //     if (e == null)
    //     {
    //         currentSpriteSet = new Sprite[0];
    //     }
    //     else
    //     {
    //         currentSpriteSet = e;
    //     }

    //     dialogueFaceSprite.gameObject.SetActive(true);
    //     if (currentSpriteSet.Length <= 0)
    //     {
    //         dialogueFaceSprite.gameObject.SetActive(false);
    //     }
    // }

    private void DialogueManager_OnToggleDialogueUI(object sender, bool e)
    {
        if (e == bIsDialogueActive)
        {
            return;
        }

        ClearDialogueText();
        SetActorName("");
        dialogueFaceSprite.gameObject.SetActive(e);
        ToggleDialogueActive(e);
    }
}
