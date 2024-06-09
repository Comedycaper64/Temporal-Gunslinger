using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueChoice
{
    public ActorSO actor;

    [TextArea]
    public string dialogueOption;
    public int correspondingDialogue;
}

[Serializable]
[CreateAssetMenu(
    fileName = "Dialogue Choice",
    menuName = "Cinematic Node/DialogueChoiceSO",
    order = 5
)]
public class DialogueChoiceSO : CinematicNode
{
    [SerializeField]
    private DialogueChoice[] dialogueChoices;

    [SerializeField]
    private Dialogue[] dialogueAnswers;

    public DialogueChoice[] GetDialogueChoices()
    {
        return dialogueChoices;
    }

    public Dialogue[] GetDialogueAnswers()
    {
        return dialogueAnswers;
    }
}
