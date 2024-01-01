using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static RewindManager Instance { get; private set; }

    private float rewindTimer = 0f;
    private bool bTimerActive = false;
    private bool bRewindActive = false;
    private Stack<IRewindable> rewindables = new Stack<IRewindable>();

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

    private void Update()
    {
        if (bTimerActive)
        {
            rewindTimer += Time.deltaTime;
        }
        else if (bRewindActive)
        {
            rewindTimer -= Time.deltaTime;
        }
    }

    public void AddRewindable(IRewindable rewindable)
    {
        if ((rewindables.Count == 0) && !bTimerActive)
        {
            ToggleTimer(true);
        }

        rewindables.Push(rewindable);
    }

    private void ToggleTimer(bool toggle)
    {
        bTimerActive = toggle;
    }

    private void ToggleRewind(bool toggle)
    {
        bRewindActive = toggle;
    }

    public bool GetRewindActive()
    {
        return bRewindActive;
    }
}
