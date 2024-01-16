using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static EventHandler<StateEnum> OnGameStateChange;

    //private PlayableDirector director;

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
        //director.Play();
        OnGameStateChange?.Invoke(this, StateEnum.active);
    }
}
