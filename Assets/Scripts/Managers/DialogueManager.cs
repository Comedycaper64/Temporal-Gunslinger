using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        TryPlayNextDialogue();
    }

    private void TryPlayNextDialogue()
    {
        Dialogue dialogueNode;
        if (dialogues.TryDequeue(out dialogueNode))
        {
            EndDialogue();
            return;
        }
        ActorSO actor = dialogueNode.actor;
    }

    private void EndDialogue()
    {
        onDialogueComplete();
    }
}
