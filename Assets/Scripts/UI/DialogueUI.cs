using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueUI : MonoBehaviour
{
    private bool bIsDialogueActive;
    private bool isTyping = false;
    private bool bPlayDialogueNoises;
    private int spriteArrayIndex = 0;
    private int charsBetweenDialogueNoises = 3;
    private float timeBetweenLetterTyping = 0.025f;
    private float spriteChangeTimer = 0f;

    [SerializeField]
    private float spriteChangeTime = 0.5f;

    private const float LOW_PITCH_RANGE = 0.75f;
    private const float HIGH_PITCH_RANGE = 1.25f;

    //private string typingSentence;
    private Coroutine typingCoroutine;
    private Coroutine actorFadeInCoroutine;
    private Action onTypingFinished;

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
    private Image actorNameFlourish;
    private Material actorFontMaterialInstance;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private Image dialogueTextbox;

    [SerializeField]
    private DialogueChoiceUI[] dialogueChoiceUI;

    public EventHandler<DialogueLog> OnNewDialogue;

    private void Awake()
    {
        DialogueManager.OnToggleDialogueUI += DialogueManager_OnToggleDialogueUI;
        DialogueManager.OnDialogue += DialogueManager_OnDialogue;
        DialogueManager.OnFinishTypingDialogue += DialogueManager_OnFinishTypingDialogue;
        DialogueManager.OnDisplayChoices += DialogueManager_OnDisplayChoices;
        DialogueChoiceUI.OnChoose += DialogueChoiceUI_OnChoose;

        ClearDialogueText();
        actorFontMaterialInstance = actorNameText.fontMaterial;
        SetActorName("", Color.black);
        dialogueFader = GetComponent<CanvasGroupFader>();
        dialogueFader.SetCanvasGroupAlpha(0f);
        dialogueFader.ToggleBlockRaycasts(false);
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

        InputManager.Instance.OnChooseDialogueAction -= InputManager_OnChooseDialogueAction;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (actorFadeInCoroutine != null)
        {
            StopCoroutine(actorFadeInCoroutine);
        }
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
    }

    private void SetActorName(string actorName, Color nameColour)
    {
        actorNameText.text = actorName;
        actorNameFlourish.color = nameColour;
        dialogueTextbox.color = nameColour;
        actorFontMaterialInstance.SetColor("_UnderlayColor", nameColour);
    }

    private void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    private void ToggleDialogueActive(bool toggle)
    {
        bIsDialogueActive = toggle;
        dialogueFader.ToggleFade(toggle);
        dialogueFader.ToggleBlockRaycasts(toggle);

        if (!toggle)
        {
            if (actorFadeInCoroutine != null)
            {
                StopCoroutine(actorFadeInCoroutine);
            }
        }
    }

    private void SetNewActor(ActorSO actorSO)
    {
        SetActorName(actorSO.GetActorName(), actorSO.GetActorNameColour());

        if (currentActor == actorSO)
        {
            return;
        }

        currentActor = actorSO;

        currentDialogueNoiseSet = actorSO.GetDialogueNoises();
        if (currentSpriteSet.Length <= 0)
        {
            SetNewActorSprites();
        }
        else
        {
            actorSpriteFader.ToggleFade(false);

            if (actorFadeInCoroutine != null)
            {
                StopCoroutine(actorFadeInCoroutine);
            }

            actorFadeInCoroutine = StartCoroutine(ActorFadeInDelay());
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
            actorSpriteFader.SetCanvasGroupAlpha(0f);
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

        dialogueText.text = dialogueUIEventArgs.sentence;
        bPlayDialogueNoises = dialogueUIEventArgs.playDialogueNoises;
        onTypingFinished = dialogueUIEventArgs.onTypingFinished;

        dialogueText.maxVisibleCharacters = 0;

        yield return null;

        string parsedText = dialogueText.GetParsedText();

        isTyping = true;
        int noiseTracker = 0;
        dialogueNoiseSource.clip = currentDialogueNoiseSet[0];

        for (int i = 0; i < parsedText.Length; i++)
        {
            dialogueText.maxVisibleCharacters++;
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

    private void ResetDialogueChoices()
    {
        for (int i = 0; i < dialogueChoiceUI.Length; i++)
        {
            dialogueChoiceUI[i].ResetChoiceColour();
            dialogueChoiceUI[i].CloseDialogueChoice();
        }

        InputManager.Instance.OnChooseDialogueAction -= InputManager_OnChooseDialogueAction;
    }

    public void ChooseDialogueOption(DialogueChoice choice)
    {
        for (int i = 0; i < dialogueChoices.Length; i++)
        {
            dialogueChoiceUI[i].CloseDialogueChoice();
        }

        InputManager.Instance.OnChooseDialogueAction -= InputManager_OnChooseDialogueAction;

        onDialogueChosen?.Invoke(this, choice);
    }

    private void DialogueManager_OnFinishTypingDialogue()
    {
        if (!isTyping)
        {
            return;
        }

        StopCoroutine(typingCoroutine);
        dialogueText.maxVisibleCharacters = dialogueText.GetParsedText().Length;
        onTypingFinished();
    }

    private void DialogueManager_OnDialogue(object sender, DialogueUIEventArgs dialogueArgs)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(dialogueArgs));
        OnNewDialogue?.Invoke(this, new DialogueLog(dialogueArgs.actorSO, dialogueArgs.sentence));
    }

    private void DialogueManager_OnToggleDialogueUI(object sender, bool e)
    {
        if (e == bIsDialogueActive)
        {
            return;
        }

        ClearDialogueText();
        SetActorName("", Color.black);
        ToggleDialogueActive(e);
    }

    private void DialogueManager_OnDisplayChoices(
        object sender,
        DialogueChoiceUIEventArgs dialogueChoiceUIArgs
    )
    {
        if (dialogueChoiceUIArgs.dialogueChoice == null)
        {
            ResetDialogueChoices();
        }
        else
        {
            dialogueChoices = dialogueChoiceUIArgs.dialogueChoice.GetDialogueChoices();
            onDialogueChosen = dialogueChoiceUIArgs.onDialogueChosen;

            SetNewActor(dialogueChoiceUIArgs.dialogueChoice.GetDialogueChoices()[0].actor);
            DisplayDialogueChoices();

            InputManager.Instance.OnChooseDialogueAction += InputManager_OnChooseDialogueAction;
        }
    }

    private void DialogueChoiceUI_OnChoose(object sender, DialogueChoice choice)
    {
        ChooseDialogueOption(choice);
    }

    private void InputManager_OnChooseDialogueAction(object sender, int choiceIndex)
    {
        if (dialogueChoices.Length < choiceIndex)
        {
            return;
        }

        ChooseDialogueOption(dialogueChoices[choiceIndex - 1]);
    }
}
