using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool bLevelActive;

    public static GameManager Instance { get; private set; }
    public static EventHandler<StateEnum> OnGameStateChange;

    [SerializeField]
    private CinemachineVirtualCamera endOfLevelCam;

    [SerializeField]
    private RewindManager rewindManager;

    [SerializeField]
    private CinematicSO levelIntroCinematic;

    [SerializeField]
    private CinematicSO levelOutroCinematic;

    public event EventHandler<bool> OnLevelLost;

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
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        rewindManager.OnResetLevel += RewindManager_OnResetLevel;

        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
    }

    private void OnDisable()
    {
        rewindManager.OnResetLevel -= RewindManager_OnResetLevel;
    }

    private void SetupLevel()
    {
        OnGameStateChange?.Invoke(this, StateEnum.idle);
        bLevelActive = true;
    }

    public void LevelStart()
    {
        if (!bLevelActive)
        {
            return;
        }

        rewindManager.StartTimer();
        OnGameStateChange?.Invoke(this, StateEnum.active);
    }

    private void ResetLevel()
    {
        //Temp reset, shouldn't reload Scene
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndLevel(Transform lastEnemy)
    {
        bLevelActive = false;
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        endOfLevelCam.gameObject.SetActive(true);
        endOfLevelCam.m_Follow = lastEnemy;
        endOfLevelCam.m_LookAt = lastEnemy;
        //switch on last enemy killed camera
        // do a focus on it
        StartCoroutine(EndOfLevelWindDown());
    }

    private IEnumerator EndOfLevelWindDown()
    {
        yield return new WaitForSeconds(2f);
        //Set rewindable movement timescale to be normal
        RewindableMovement.UpdateMovementTimescale(1f);
        CinematicManager.Instance.PlayCinematic(levelOutroCinematic, LoadNextLevel);
    }

    public void LevelLost()
    {
        TimeManager.SetPausedTime();
        OnLevelLost?.Invoke(this, true);
    }

    public void UndoLevelLost()
    {
        OnLevelLost?.Invoke(this, false);
    }

    public bool IsLevelActive()
    {
        return bLevelActive;
    }

    private void RewindManager_OnResetLevel()
    {
        Debug.Log("RESET!");
        if (!bLevelActive)
        {
            return;
        }

        ResetLevel();
    }
}
