using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    // private CameraMode endOfTimelineCamera;
    // private ActorSO endOfTimelineActor;

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

        Type nodeType = cinematicNode.GetType();

        if (nodeType == typeof(DialogueSO))
        {
            dialogueManager.PlayDialogue(cinematicNode as DialogueSO, TryPlayNextNode);
        }
        else if (nodeType == typeof(DialogueChoiceSO))
        {
            dialogueManager.DisplayChoices(cinematicNode as DialogueChoiceSO, TryPlayNextNode);
        }
        else if (nodeType == typeof(PlaySFXSO))
        {
            PlaySFXSO sfx = cinematicNode as PlaySFXSO;
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
            HandleMovementNode(cinematicNode);
        }
        else if (nodeType == typeof(TimelineSO))
        {
            TimelineSO timeline = cinematicNode as TimelineSO;
            int timelineIndex = timeline.directorIndex;
            // endOfTimelineCamera = timeline.endOfTimelineCamera;
            // endOfTimelineActor = timeline.endOfTimelineActor;
            timelineDirectors[timelineIndex].Play();
            timelineDirectors[timelineIndex].stopped += TimelineFinished;
        }
        else if (nodeType == typeof(UIChangeSO))
        {
            UIChangeSO uIChange = cinematicNode as UIChangeSO;
            uIChange.onFaded = TryPlayNextNode;
            OnFadeToBlackToggle?.Invoke(this, uIChange);
        }
        else if (nodeType == typeof(SceneChangeSO))
        {
            SceneChangeSO sceneChangeSO = cinematicNode as SceneChangeSO;

            scrollingHallway.ToggleScroll(sceneChangeSO.startScrollingWalk);

            if (audioManager)
            {
                if (sceneChangeSO.musicTrackChange)
                {
                    audioManager.SetMusicTrack(sceneChangeSO.musicTrackChange);
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

    private void HandleMovementNode(CinematicNode cinematicNode)
    {
        ActorMovementSO actorMovement = cinematicNode as ActorMovementSO;
        Animator animator = actorAnimatorMapper.GetAnimators(
            actorMovement.actor.GetAnimatorController()
        )[actorMovement.actorIndex];
        ActorMover mover = animator.GetComponent<ActorMover>();

        mover.MoveActor(actorMovement, TryPlayNextNode);
    }

    private void EndCinematic()
    {
        OnCinematicFinished?.Invoke();
    }

    private void TimelineFinished(PlayableDirector director)
    {
        // Set active camera
        //dialogueManager.SetDialogueCamera(endOfTimelineCamera, endOfTimelineActor);
        TryPlayNextNode();
    }
}
