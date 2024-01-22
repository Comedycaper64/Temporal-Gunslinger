using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    //Gets in a CinematicSO
    //Determines what type of node it is
    //Handles node appropriately
    //Subscribes to OnNodeComplete event and waits for it to call before continuing
    //Goes through list
    //Hands off Dialogues to Dialogue Manager
    //Hands off Timelines to Timeline manager
    //Hands off music change to audio manager
    private Queue<CinematicNode> cinematicNodes;
    private Action OnCinematicFinished;
    private DialogueManager dialogueManager;
    public static CinematicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one CinematicManager! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        dialogueManager = DialogueManager.Instance;
    }

    public void PlayCinematic(CinematicSO cinematicSO, Action OnCinematicFinished)
    {
        this.OnCinematicFinished = OnCinematicFinished;
        cinematicNodes = new Queue<CinematicNode>(cinematicSO.GetCinematicNodes());
        TryPlayNextNode();
    }

    private void TryPlayNextNode()
    {
        CinematicNode cinematicNode;

        if (!cinematicNodes.TryDequeue(out cinematicNode))
        {
            EndCinematic();
            return;
        }

        if (cinematicNode.GetType() == typeof(DialogueSO))
        {
            dialogueManager.PlayDialogue(cinematicNode as DialogueSO, TryPlayNextNode);
        }
        else
        {
            Debug.Log("Error, undefined type");
        }
    }

    private void EndCinematic()
    {
        OnCinematicFinished?.Invoke();
    }
}
