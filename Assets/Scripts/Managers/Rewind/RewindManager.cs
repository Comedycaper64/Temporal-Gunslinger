using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    private float rewindTimer = 0f;
    private bool bTimerActive = false;
    private bool bRewindActive = false;
    private Stack<RewindableAction> rewindableActions = new Stack<RewindableAction>();
    private HashSet<RewindableMovement> rewindableMovements;
    private InputManager input;

    private void Start()
    {
        input = InputManager.Instance;
        rewindableMovements = RewindableMovement.Instances;
        RewindableAction.OnRewindableActionCreated += RewindableActionCreated;
    }

    private void OnDisable()
    {
        RewindableAction.OnRewindableActionCreated -= RewindableActionCreated;
    }

    private void Update()
    {
        if (!bTimerActive)
        {
            return;
        }

        if (bRewindActive != input.GetIsRewinding())
        {
            ToggleRewind(input.GetIsRewinding());
        }

        if (!bRewindActive)
        {
            rewindTimer += Time.deltaTime;
        }
        else
        {
            rewindTimer -= Time.deltaTime;
            TryUndoRewindables();
        }
    }

    private void TryUndoRewindables()
    {
        bool bNoOutstandingRewindables = false;
        while (!bNoOutstandingRewindables)
        {
            if (!rewindableActions.TryPeek(out RewindableAction rewindable))
            {
                bNoOutstandingRewindables = true;
                ResetManager();
            }
            else
            {
                float timestamp = rewindable.GetTimestamp();
                if ((timestamp < rewindTimer) && (rewindable.GetType() != typeof(StopTime)))
                {
                    bNoOutstandingRewindables = true;
                }
                else
                {
                    Debug.Log("Undo");
                    rewindableActions.Pop();

                    rewindable.Undo();
                }
            }
        }
    }

    public void AddRewindable(RewindableAction rewindable)
    {
        if (!bTimerActive)
        {
            return;
        }

        rewindable.SetTimestamp(rewindTimer);
        rewindableActions.Push(rewindable);
        Debug.Log("Timestamp: " + rewindTimer + ", Object: " + rewindable.GetType());
    }

    private void ToggleTimer(bool toggle)
    {
        bTimerActive = toggle;
    }

    private void ToggleRewind(bool toggle)
    {
        bRewindActive = toggle;
        ToggleRewindableMovements(toggle);
        //OnRewindToggled?.Invoke(this, bRewindActive);
    }

    private void ToggleRewindableMovements(bool toggle)
    {
        foreach (RewindableMovement rewindableMovement in rewindableMovements)
        {
            if (toggle)
            {
                rewindableMovement.BeginRewind();
            }
            else
            {
                rewindableMovement.BeginPlay();
            }
        }
    }

    private void ResetManager()
    {
        ToggleTimer(false);
        ToggleRewind(false);
        rewindTimer = 0f;
    }

    public bool GetRewindActive()
    {
        return bRewindActive;
    }

    public bool GetTimerActive()
    {
        return bTimerActive;
    }

    public void StartTimer()
    {
        ToggleTimer(true);
    }

    private void RewindableActionCreated(object sender, RewindableAction newRewindable)
    {
        AddRewindable(newRewindable);
    }
}
