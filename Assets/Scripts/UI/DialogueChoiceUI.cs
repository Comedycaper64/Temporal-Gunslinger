using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueChoiceUI : MonoBehaviour
{
    private CanvasGroup choiceCanvasGroup;
    private DialogueChoice dialogueChoice;
    public static event EventHandler<int> OnChoose;

    [SerializeField]
    private TextMeshProUGUI choiceText;

    private void Awake()
    {
        choiceCanvasGroup = GetComponent<CanvasGroup>();
        CloseDialogueChoice();
    }

    public void SetupDialogueChoice(DialogueChoice dialogueChoice)
    {
        this.dialogueChoice = dialogueChoice;
        choiceText.text = dialogueChoice.dialogueOption;
        choiceCanvasGroup.alpha = 1f;
        choiceCanvasGroup.interactable = true;
    }

    public void ChooseDialogueOption()
    {
        OnChoose?.Invoke(this, dialogueChoice.correspondingDialogue);
    }

    public void CloseDialogueChoice()
    {
        choiceCanvasGroup.alpha = 0f;
        choiceCanvasGroup.interactable = false;
    }
}
