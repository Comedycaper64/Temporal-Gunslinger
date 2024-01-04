using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //enum tracker for inactive / active game state
    // fires off event when switching between the two
    //state machine implement an abstract SetupDictionary for setting up what states correspond to inactive / active
    //Player / enemy statemachine then have to setup their active / inactive states that they'll switch to depending on game manager
    public static GameManager Instance { get; private set; }
    public static EventHandler<StateEnum> OnGameStateChange;

    [SerializeField]
    private RewindManager rewindManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void LevelStart()
    {
        //Would like to stop using the instance
        rewindManager.StartTimer();

        OnGameStateChange?.Invoke(this, StateEnum.active);
    }

    //Method for setting level into inactive state again, signal sent from rewind manager when rewindable stack is empty
}
