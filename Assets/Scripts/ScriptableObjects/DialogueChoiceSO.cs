using System;
using UnityEngine;

[Serializable]
public struct DialogueAnswer
{
    public Dialogue[] dialogueAnswers;
}

[Serializable]
public struct DialogueChoice
{
    public ActorSO actor;

    [TextArea]
    public string dialogueOption;
    public int correspondingDialogue;
    public bool loopBackToChoice;
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
    private DialogueAnswer[] dialogueAnswers;

    public DialogueChoice[] GetDialogueChoices()
    {
        return dialogueChoices;
    }

    public DialogueAnswer[] GetDialogueAnswers()
    {
        return dialogueAnswers;
    }
}
