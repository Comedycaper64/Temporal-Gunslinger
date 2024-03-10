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
    private ActorAnimatorMapper actorAnimatorMapper;
    private ActorMover actorMover;
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
        actorAnimatorMapper = dialogueManager.GetComponent<ActorAnimatorMapper>();
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
        else if (cinematicNode.GetType() == typeof(ActorMovementSO))
        {
            ActorMovementSO actorMovement = cinematicNode as ActorMovementSO;
            Animator animator = actorAnimatorMapper.GetAnimators(
                actorMovement.actor.GetAnimatorController()
            )[0];
            actorMover.MoveActor(actorMovement, animator.GetComponent<IMover>(), TryPlayNextNode);
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
