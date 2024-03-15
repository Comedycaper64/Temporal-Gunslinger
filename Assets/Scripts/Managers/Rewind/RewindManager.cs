using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static bool bRewinding;

    private float rewindTimer = 0f;
    private float resetTimer;
    private const float RESET_TIME = 1f;
    private bool bCanReset = true;
    private bool bCanRewind = true;
    private bool bTimerActive = false;
    private bool bRewindActive = false;
    private Stack<RewindableAction> rewindableActions = new Stack<RewindableAction>();
    private Stack<RewindableAction> priorityActions = new Stack<RewindableAction>();
    private HashSet<RewindableMovement> rewindableMovements;
    private InputManager input;

    public event Action OnResetLevel;

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
        CheckIsResetting();

        if (!bTimerActive)
        {
            return;
        }

        CheckIsRewinding();

        IncrementRewindTimer();
    }

    private void CheckIsRewinding()
    {
        if (!bCanRewind)
        {
            return;
        }

        if (bRewindActive != input.GetIsRewinding())
        {
            ToggleRewind(input.GetIsRewinding());
        }
    }

    private void IncrementRewindTimer()
    {
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

    private void CheckIsResetting()
    {
        if (!bCanReset)
        {
            return;
        }

        if (input.GetIsResetting())
        {
            resetTimer += Time.unscaledDeltaTime;
            if (resetTimer > RESET_TIME)
            {
                OnResetLevel?.Invoke();
                resetTimer = 0f;
            }
        }
        else
        {
            resetTimer = 0f;
        }
    }

    private void TryUndoRewindables()
    {
        bool bNoOutstandingRewindables = false;
        while (!bNoOutstandingRewindables)
        {
            if (priorityActions.TryPeek(out RewindableAction priorityRewindable))
            {
                priorityActions.Pop();
                priorityRewindable.Undo();
                if (priorityRewindable.GetType() == typeof(StopTime))
                {
                    rewindTimer = priorityRewindable.GetTimestamp();
                }
            }

            if (!rewindableActions.TryPeek(out RewindableAction rewindable))
            {
                bNoOutstandingRewindables = true;
                ResetManager();
            }
            else
            {
                float timestamp = rewindable.GetTimestamp();
                if (timestamp < rewindTimer)
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
        if (rewindable.IsPriority())
        {
            priorityActions.Push(rewindable);
        }
        else
        {
            rewindableActions.Push(rewindable);
        }

        Debug.Log("Timestamp: " + rewindTimer + ", Object: " + rewindable.GetType());
    }

    private void ToggleTimer(bool toggle)
    {
        bTimerActive = toggle;
    }

    private void ToggleRewind(bool toggle)
    {
        bRewindActive = toggle;
        bRewinding = toggle;
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

    public void ResetManager()
    {
        ToggleTimer(false);
        ToggleRewind(false);
        priorityActions = new Stack<RewindableAction>();
        rewindableActions = new Stack<RewindableAction>();
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

    public void ToggleCanRewind(bool toggle)
    {
        bCanRewind = toggle;
    }

    public void ToggleCanReset(bool toggle)
    {
        bCanReset = toggle;
    }

    private void RewindableActionCreated(object sender, RewindableAction newRewindable)
    {
        AddRewindable(newRewindable);
    }
}
