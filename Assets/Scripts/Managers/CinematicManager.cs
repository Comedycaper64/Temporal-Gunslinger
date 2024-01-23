using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    private Queue<CinematicNode> cinematicNodes;
    private Action OnCinematicFinished;

    [SerializeField]
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
