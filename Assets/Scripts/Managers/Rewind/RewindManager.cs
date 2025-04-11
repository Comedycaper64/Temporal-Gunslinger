using System;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static bool bRewinding;
    public static bool bTimerActive;

    private double rewindTimer = 0f;

    private float resetTimer;
    private const float RESET_TIME = 1f;
    private bool bCanReset = true;
    private bool bCanRewind = true;

    private bool bRewindActive = false;
    private bool bTurboActive = false;
    private Stack<RewindableAction> rewindableActions = new Stack<RewindableAction>();
    private Stack<RewindableAction> priorityActions = new Stack<RewindableAction>();
    private HashSet<RewindableMovement> rewindableMovements;
    private InputManager input;

    [SerializeField]
    private AudioClip rewindSFX;

    [SerializeField]
    private AudioClip turboSFX;
    private AudioSource activeRewindSFX;
    private AudioSource activeTurboSFX;

    public event Action OnResetLevel;
    public event Action OnRewindToStart;
    public static EventHandler<bool> OnRewindToggle;
    public static EventHandler<float> OnRestartTimerChanged;

    private void Start()
    {
        bRewinding = false;
        bTimerActive = false;
        input = InputManager.Instance;
        rewindableMovements = RewindableMovement.Instances;
        RewindableMovement.UpdateMovementTimescale(1f);
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
        CheckIsResetting();

        CheckIsRewinding();

        CheckIsTurbo();

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

    private void CheckIsTurbo()
    {
        if (!bCanRewind)
        {
            return;
        }

        if (bTurboActive != input.GetIsTurbo())
        {
            ToggleTurbo(input.GetIsTurbo());
        }
    }

    private void IncrementRewindTimer()
    {
        if (!bRewindActive)
        {
            rewindTimer += Time.deltaTime * RewindableMovement.GetTimescale();
        }
        else
        {
            rewindTimer -= Time.deltaTime * RewindableMovement.GetTimescale();
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

            OnRestartTimerChanged?.Invoke(this, resetTimer);

            if (resetTimer > RESET_TIME)
            {
                OnResetLevel?.Invoke();
                resetTimer = 0f;
                OnRestartTimerChanged?.Invoke(this, resetTimer);
            }
        }
        else if (resetTimer > 0f)
        {
            resetTimer = 0f;
            OnRestartTimerChanged?.Invoke(this, resetTimer);
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
                OnRewindToStart?.Invoke();
                ResetManager(false);
            }
            else
            {
                double timestamp = rewindable.GetTimestamp();
                if (timestamp < rewindTimer)
                {
                    bNoOutstandingRewindables = true;
                }
                else
                {
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
    }

    private void ToggleTimer(bool toggle)
    {
        bTimerActive = toggle;
    }

    private void ToggleRewind(bool toggle)
    {
        bRewindActive = toggle;
        bRewinding = toggle;
        ToggleTurbo(false);
        ToggleRewindableMovements(toggle);
        OnRewindToggle?.Invoke(this, bRewindActive);

        if (toggle && !activeRewindSFX)
        {
            activeRewindSFX = AudioManager.PlaySFX(
                rewindSFX,
                0.5f,
                0,
                Camera.main.transform.position,
                false,
                true,
                true
            );
        }
    }

    private void ToggleTurbo(bool toggle)
    {
        bTurboActive = toggle;
        TimeManager.SetTurboTime(toggle);

        if (toggle && !activeTurboSFX)
        {
            activeTurboSFX = AudioManager.PlaySFX(
                turboSFX,
                0.3f,
                0,
                Camera.main.transform.position,
                false
            );
        }
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

    public void ResetManager(bool endOfLevel)
    {
        ToggleTimer(false);
        ToggleRewind(false);
        ToggleTurbo(false);

        if (!endOfLevel)
        {
            while (priorityActions.Count > 0)
            {
                RewindableAction priorityAction = priorityActions.Pop();
                priorityAction.Undo();
            }

            while (rewindableActions.Count > 0)
            {
                RewindableAction rewindableAction = rewindableActions.Pop();
                rewindableAction.Undo();
            }
        }

        priorityActions = new Stack<RewindableAction>();
        rewindableActions = new Stack<RewindableAction>();
        rewindTimer = 0f;

        RewindableMovement.UpdateMovementTimescale(1f);
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
