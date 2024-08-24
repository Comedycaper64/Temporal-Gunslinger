using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueChoiceUI : MonoBehaviour
{
    private CanvasGroup choiceCanvasGroup;
    private CanvasGroupFader canvasFader;
    private DialogueChoice dialogueChoice;
    public static event EventHandler<DialogueChoice> OnChoose;

    [SerializeField]
    private TextMeshProUGUI choiceText;

    [SerializeField]
    private Color unreadColour;

    [SerializeField]
    private Color readColour;

    private void Awake()
    {
        choiceCanvasGroup = GetComponent<CanvasGroup>();
        canvasFader = GetComponent<CanvasGroupFader>();
        CloseDialogueChoice();
    }

    public void SetupDialogueChoice(DialogueChoice dialogueChoice)
    {
        this.dialogueChoice = dialogueChoice;
        choiceText.text = dialogueChoice.dialogueOption;
        canvasFader.ToggleFade(true);
        StartCoroutine(InteractDelay());
    }

    private IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(0.5f);
        choiceCanvasGroup.interactable = true;
    }

    public void ChooseDialogueOption()
    {
        choiceText.color = readColour;

        OnChoose?.Invoke(this, dialogueChoice);
    }

    public void CloseDialogueChoice()
    {
        canvasFader.ToggleFade(false);
        choiceCanvasGroup.interactable = false;
    }

    public void ResetChoiceColour()
    {
        choiceText.color = unreadColour;
    }
}
