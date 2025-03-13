using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    // private CameraMode endOfTimelineCamera;
    // private ActorSO endOfTimelineActor;

    private bool skipping = false;
    private bool exitFlag = false;
    private CinematicNode currentCinematicNode;
    private Queue<CinematicNode> cinematicNodes;
    private Action OnCinematicFinished;

    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private ScrollingHallway scrollingHallway;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private PlayableDirector[] timelineDirectors;

    private ActorAnimatorMapper actorAnimatorMapper;

    public static EventHandler<UIChangeSO> OnFadeToBlackToggle;

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
        exitFlag = false;

        PauseMenuUI.OnSkipCutscene += SkipCinematic;
        PauseMenuUI.OnExitLevel += ExitCleanup;
    }

    private void OnDisable()
    {
        PauseMenuUI.OnSkipCutscene -= SkipCinematic;
        PauseMenuUI.OnExitLevel -= ExitCleanup;
    }

    public void PlayCinematic(CinematicSO cinematicSO, Action OnCinematicFinished)
    {
        this.OnCinematicFinished = OnCinematicFinished;
        cinematicNodes = new Queue<CinematicNode>(cinematicSO.GetCinematicNodes());
        TryPlayNextNode();
    }

    public void SkipCinematic()
    {
        if ((cinematicNodes.Count <= 0) && !currentCinematicNode)
        {
            return;
        }

        skipping = true;
        TrySkipCurrentNode();
    }

    private void TryPlayNextNode()
    {
        if (!cinematicNodes.TryDequeue(out currentCinematicNode))
        {
            EndCinematic();
            return;
        }

        Type nodeType = currentCinematicNode.GetType();

        //Debug.Log("Playing Node: " + nodeType.ToString());

        if (nodeType == typeof(DialogueSO))
        {
            dialogueManager.PlayDialogue(currentCinematicNode as DialogueSO, TryPlayNextNode);
        }
        else if (nodeType == typeof(DialogueChoiceSO))
        {
            dialogueManager.DisplayChoices(
                currentCinematicNode as DialogueChoiceSO,
                TryPlayNextNode
            );
        }
        else if (nodeType == typeof(PlaySFXSO))
        {
            PlaySFXSO sfx = currentCinematicNode as PlaySFXSO;
            AudioManager.PlaySFX(
                sfx.soundEffect,
                sfx.sfxVolume,
                sfx.sfxPitch,
                Camera.main.transform.position
            );
            TryPlayNextNode();
        }
        else if (nodeType == typeof(ActorMovementSO))
        {
            HandleMovementNode(currentCinematicNode as ActorMovementSO);
        }
        else if (nodeType == typeof(TimelineSO))
        {
            TimelineSO timeline = currentCinematicNode as TimelineSO;
            int timelineIndex = timeline.directorIndex;
            // endOfTimelineCamera = timeline.endOfTimelineCamera;
            // endOfTimelineActor = timeline.endOfTimelineActor;
            timelineDirectors[timelineIndex].Play();
            timelineDirectors[timelineIndex].stopped += TimelineFinished;
        }
        else if (nodeType == typeof(UIChangeSO))
        {
            UIChangeSO uIChange = currentCinematicNode as UIChangeSO;
            uIChange.onFaded = TryPlayNextNode;
            OnFadeToBlackToggle?.Invoke(this, uIChange);
        }
        else if (nodeType == typeof(SceneChangeSO))
        {
            SceneChangeSO sceneChangeSO = currentCinematicNode as SceneChangeSO;

            if (sceneChangeSO.startScrollingWalk)
            {
                scrollingHallway.ToggleScroll(sceneChangeSO.startScrollingWalk);
            }

            if (audioManager)
            {
                if (sceneChangeSO.musicTrackChange)
                {
                    audioManager.SetMusicTrack(
                        sceneChangeSO.musicTrackChange,
                        sceneChangeSO.musicTrackChangeBacking
                    );
                }
                if (sceneChangeSO.fadeInMusic)
                {
                    audioManager.FadeInMusic();
                }

                if (sceneChangeSO.fadeOutMusic)
                {
                    audioManager.FadeOutMusic();
                }
            }

            TryPlayNextNode();
        }
        else
        {
            Debug.Log("Error, undefined type");
        }
    }

    private void TrySkipCurrentNode()
    {
        Type nodeType = currentCinematicNode.GetType();

        if (nodeType == typeof(DialogueSO))
        {
            dialogueManager.SkipCurrentDialogue();
            TrySkipNextNode();
        }
        else if (nodeType == typeof(DialogueChoiceSO))
        {
            dialogueManager.SkipCurrentChoice();
            TrySkipNextNode();
        }
        else if (nodeType == typeof(ActorMovementSO))
        {
            SkipCurrentMovementNode(currentCinematicNode as ActorMovementSO);
            TrySkipNextNode();
        }
        else if (nodeType == typeof(TimelineSO))
        {
            TimelineSO timeline = currentCinematicNode as TimelineSO;
            int timelineIndex = timeline.directorIndex;
            timelineDirectors[timelineIndex].time =
                timelineDirectors[timelineIndex].duration - 0.001f;

            timelineDirectors[timelineIndex].Play();
            timelineDirectors[timelineIndex].stopped += TimelineSkipped;
        }
    }

    private void TrySkipNextNode()
    {
        CinematicNode cinematicNode;

        if (!cinematicNodes.TryDequeue(out cinematicNode))
        {
            skipping = false;
            EndCinematic();
            return;
        }

        Type nodeType = cinematicNode.GetType();

        if (nodeType == typeof(DialogueSO))
        {
            dialogueManager.SkipDialogue(cinematicNode as DialogueSO, TrySkipNextNode);
        }
        else if (nodeType == typeof(ActorMovementSO))
        {
            SkipMovementNode(cinematicNode as ActorMovementSO);
        }
        else if (nodeType == typeof(TimelineSO))
        {
            TimelineSO timeline = cinematicNode as TimelineSO;
            int timelineIndex = timeline.directorIndex;
            timelineDirectors[timelineIndex].time =
                timelineDirectors[timelineIndex].duration - 0.001f;

            timelineDirectors[timelineIndex].Play();
            timelineDirectors[timelineIndex].stopped += TimelineSkipped;
        }
        else if (nodeType == typeof(UIChangeSO))
        {
            UIChangeSO uIChange = cinematicNode as UIChangeSO;
            //uIChange.onFaded = TrySkipNextNode;
            OnFadeToBlackToggle?.Invoke(this, uIChange);
            TrySkipNextNode();
        }
        else
        {
            TrySkipNextNode();
        }
    }

    private ActorMover GetActorMover(ActorMovementSO movementNode)
    {
        Animator animator = actorAnimatorMapper.GetAnimators(
            movementNode.actor.GetAnimatorController()
        )[movementNode.actorIndex];
        ActorMover mover = animator.GetComponent<ActorMover>();
        return mover;
    }

    private void HandleMovementNode(ActorMovementSO movementNode)
    {
        GetActorMover(movementNode).MoveActor(movementNode, TryPlayNextNode);
    }

    private void SkipMovementNode(ActorMovementSO movementNode)
    {
        GetActorMover(movementNode).SkipActorMovement(movementNode);
        TrySkipNextNode();
    }

    private void SkipCurrentMovementNode(ActorMovementSO movementNode)
    {
        GetActorMover(movementNode).SkipCurrentActorMovement();
    }

    private void EndCinematic()
    {
        currentCinematicNode = null;
        OnCinematicFinished?.Invoke();
    }

    private void TimelineFinished(PlayableDirector director)
    {
        if (exitFlag)
        {
            return;
        }

        if (skipping)
        {
            return;
        }

        TryPlayNextNode();
    }

    private void TimelineSkipped(PlayableDirector director)
    {
        TrySkipNextNode();
    }

    private void ExitCleanup()
    {
        exitFlag = true;
    }
}
