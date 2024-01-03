using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static RewindManager Instance { get; private set; }

    private float rewindTimer = 0f;
    private bool bTimerActive = false;
    private bool bRewindActive = false;
    private Stack<RewindableAction> rewindables = new Stack<RewindableAction>();
    private InputManager input;
    public event EventHandler<bool> OnRewindToggled;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one RewindManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        input = InputManager.Instance;
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
            if (!rewindables.TryPeek(out RewindableAction rewindable))
            {
                bNoOutstandingRewindables = true;
                ResetManager();
            }
            else
            {
                float timestamp = rewindable.GetTimestamp();
                if (timestamp <= rewindTimer)
                {
                    bNoOutstandingRewindables = true;
                }
                else
                {
                    Debug.Log("Undo");
                    rewindables.Pop();
                    rewindable.Undo();
                }
            }
        }
    }

    public void AddRewindable(RewindableAction rewindable)
    {
        if ((rewindables.Count == 0) && !bTimerActive)
        {
            ToggleTimer(true);
        }
        rewindable.SetTimestamp(rewindTimer);
        rewindables.Push(rewindable);
        Debug.Log("Timestamp: " + rewindTimer + ", Object: " + rewindable.GetType());
    }

    private void ToggleTimer(bool toggle)
    {
        bTimerActive = toggle;
    }

    private void ToggleRewind(bool toggle)
    {
        bRewindActive = toggle;
        OnRewindToggled?.Invoke(this, bRewindActive);
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
}
