using System;
using System.Collections;
using System.Collections.Generic;
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
        OnChoose?.Invoke(this, dialogueChoice);
    }

    public void CloseDialogueChoice()
    {
        canvasFader.ToggleFade(false);
        choiceCanvasGroup.interactable = false;
    }
}
