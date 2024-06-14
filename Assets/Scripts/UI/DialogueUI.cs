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

    //private bool bDialogueActiveChanged;
    private bool isTyping = false;
    private bool bPlayDialogueNoises;
    private int spriteArrayIndex = 0;
    private int charsBetweenDialogueNoises = 3;
    private float timeBetweenLetterTyping = 0.025f;
    private float spriteChangeTimer = 0f;

    [SerializeField]
    private float spriteChangeTime = 0.5f;

    //private float textBoxFadeSpeed = 2.5f;
    private const float LOW_PITCH_RANGE = 0.75f;
    private const float HIGH_PITCH_RANGE = 1.25f;
    private string typingSentence;
    private Coroutine typingCoroutine;
    private Action onTypingFinished;

    //private CanvasGroup dialogueCanvasGroup;
    private CanvasGroupFader dialogueFader;

    [SerializeField]
    private CanvasGroupFader actorSpriteFader;
    private DialogueChoice[] dialogueChoices;
    private EventHandler<DialogueChoice> onDialogueChosen;

    [SerializeField]
    private Image dialogueFaceSprite;
    private ActorSO currentActor;
    private Sprite[] currentSpriteSet = new Sprite[0];
    private AudioClip[] currentDialogueNoiseSet;
    private AudioSource dialogueNoiseSource;

    [SerializeField]
    private TextMeshProUGUI actorNameText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private DialogueChoiceUI[] dialogueChoiceUI;

    private void Awake()
    {
        DialogueManager.OnToggleDialogueUI += DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue += DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue += DialogueManager_OnFinishTypingDialogue;
        DialogueManager.OnDisplayChoices += DialogueManager_OnDisplayChoices;
        DialogueChoiceUI.OnChoose += DialogueChoiceUI_OnChoose;
        //DialogueManager.OnChangeSprite += DialogueManager_OnChangeSprite;

        ClearDialogueText();
        SetActorName("");
        //textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 0f);
        //dialogueCanvasGroup = GetComponent<CanvasGroup>();
        dialogueFader = GetComponent<CanvasGroupFader>();
        dialogueFader.SetCanvasGroupAlpha(0f);
        actorSpriteFader.SetCanvasGroupAlpha(0f);
        dialogueNoiseSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        DialogueManager.OnToggleDialogueUI -= DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue -= DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue -= DialogueManager_OnFinishTypingDialogue;
        DialogueManager.OnDisplayChoices -= DialogueManager_OnDisplayChoices;
        DialogueChoiceUI.OnChoose -= DialogueChoiceUI_OnChoose;
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

        // if (bDialogueActiveChanged)
        // {
        //     FadeUI();
        // }
    }

    // private void FadeUI()
    // {
    //     if (bIsDialogueActive)
    //     {
    //         dialogueCanvasGroup.alpha += textBoxFadeSpeed * Time.deltaTime;

    //         if (dialogueCanvasGroup.alpha >= 1f)
    //         {
    //             bDialogueActiveChanged = false;
    //         }
    //     }
    //     else
    //     {
    //         dialogueCanvasGroup.alpha -= textBoxFadeSpeed * Time.deltaTime;

    //         if (dialogueCanvasGroup.alpha <= 0f)
    //         {
    //             bDialogueActiveChanged = false;
    //         }
    //     }
    // }

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
        dialogueFader.ToggleFade(toggle);
        //bDialogueActiveChanged = true;
    }

    private void SetNewActor(ActorSO actorSO)
    {
        SetActorName(actorSO.GetActorName());

        if (currentActor == actorSO)
        {
            return;
        }

        currentActor = actorSO;
        actorSpriteFader.ToggleFade(false);

        currentDialogueNoiseSet = actorSO.GetDialogueNoises();
        if (currentSpriteSet.Length <= 0)
        {
            SetNewActorSprites();
        }
        else
        {
            StartCoroutine(ActorFadeInDelay());
        }
    }

    private void SetNewActorSprites()
    {
        Sprite[] spriteSet = currentActor.GetActorSprites();
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
            //dialogueFaceSprite.gameObject.SetActive(false);
        }
        else
        {
            actorSpriteFader.ToggleFade(true);
        }

        spriteChangeTime = currentActor.GetSpriteSwitchTime();
    }

    private IEnumerator ActorFadeInDelay()
    {
        yield return new WaitForSeconds(0.35f);
        SetNewActorSprites();
    }

    private IEnumerator TypeSentence(DialogueUIEventArgs dialogueUIEventArgs)
    {
        SetNewActor(dialogueUIEventArgs.actorSO);

        typingSentence = dialogueUIEventArgs.sentence;
        bPlayDialogueNoises = dialogueUIEventArgs.playDialogueNoises;
        onTypingFinished = dialogueUIEventArgs.onTypingFinished;

        ClearDialogueText();

        isTyping = true;
        int noiseTracker = 0;
        dialogueNoiseSource.clip = currentDialogueNoiseSet[0];
        foreach (char letter in typingSentence.ToCharArray())
        {
            dialogueText.text += letter;

            if (bPlayDialogueNoises)
            {
                noiseTracker++;
                if (noiseTracker >= charsBetweenDialogueNoises)
                {
                    // dialogueNoiseSource.clip = currentDialogueNoiseSet[
                    //     Random.Range(0, currentDialogueNoiseSet.Length)
                    // ];
                    dialogueNoiseSource.pitch = Random.Range(LOW_PITCH_RANGE, HIGH_PITCH_RANGE);
                    dialogueNoiseSource.Play();
                    noiseTracker = 0;
                }
            }

            yield return new WaitForSeconds(timeBetweenLetterTyping);
        }
        isTyping = false;
        onTypingFinished();
    }

    private void DisplayDialogueChoices()
    {
        for (int i = 0; i < dialogueChoices.Length; i++)
        {
            dialogueChoiceUI[i].SetupDialogueChoice(dialogueChoices[i]);
        }
    }

    public void ChooseDialogueOption(DialogueChoice choice)
    {
        for (int i = 0; i < dialogueChoices.Length; i++)
        {
            dialogueChoiceUI[i].CloseDialogueChoice();
        }

        onDialogueChosen?.Invoke(this, choice);
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
        actorSpriteFader.ToggleFade(true);
        ToggleDialogueActive(e);
    }

    private void DialogueManager_OnDisplayChoices(
        object sender,
        DialogueChoiceUIEventArgs dialogueChoiceUIArgs
    )
    {
        dialogueChoices = dialogueChoiceUIArgs.dialogueChoice.GetDialogueChoices();
        onDialogueChosen = dialogueChoiceUIArgs.onDialogueChosen;

        SetNewActor(dialogueChoiceUIArgs.dialogueChoice.GetDialogueChoices()[0].actor);
        DisplayDialogueChoices();
    }

    private void DialogueChoiceUI_OnChoose(object sender, DialogueChoice choice)
    {
        ChooseDialogueOption(choice);
    }
}
