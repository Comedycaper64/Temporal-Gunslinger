using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static EventHandler<StateEnum> OnGameStateChange;

    [SerializeField]
    private RewindManager rewindManager;

    [SerializeField]
    private CinematicSO levelIntroCinematic;

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

    private void Start()
    {
        //OnGameStateChange?.Invoke(this, StateEnum.inactive);
        //CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
        OnGameStateChange?.Invoke(this, StateEnum.idle);
    }

    private void SetupLevel()
    {
        OnGameStateChange?.Invoke(this, StateEnum.idle);
    }

    public void LevelStart()
    {
        rewindManager.StartTimer();
        OnGameStateChange?.Invoke(this, StateEnum.active);
    }
}
