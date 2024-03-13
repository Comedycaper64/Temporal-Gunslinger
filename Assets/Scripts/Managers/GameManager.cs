using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    protected bool bLevelActive;

    public static GameManager Instance { get; private set; }
    public static EventHandler<StateEnum> OnGameStateChange;

    [SerializeField]
    protected CinemachineVirtualCamera endOfLevelCam;

    [SerializeField]
    protected RewindManager rewindManager;

    [SerializeField]
    protected CinematicSO levelIntroCinematic;

    [SerializeField]
    protected CinematicSO levelOutroCinematic;

    public event EventHandler<bool> OnLevelLost;

    private void Awake()
    {
        Debug.Log("ayaya");
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public virtual void Start()
    {
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        rewindManager.OnResetLevel += RewindManager_OnResetLevel;

        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
    }

    private void OnDisable()
    {
        rewindManager.OnResetLevel -= RewindManager_OnResetLevel;
    }

    public virtual void SetupLevel()
    {
        OnGameStateChange?.Invoke(this, StateEnum.idle);
        bLevelActive = true;
    }

    public virtual void LevelStart()
    {
        if (!bLevelActive)
        {
            return;
        }

        rewindManager.StartTimer();
        OnGameStateChange?.Invoke(this, StateEnum.active);
    }

    protected void ResetLevel()
    {
        //Temp reset, shouldn't reload Scene
        SceneManager.LoadScene(0);
    }

    protected void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public virtual void EndLevel(Transform lastEnemy)
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

    public virtual IEnumerator EndOfLevelWindDown()
    {
        yield return new WaitForSeconds(2f);
        //Set rewindable movement timescale to be normal
        RewindableMovement.UpdateMovementTimescale(1f);
        CinematicManager.Instance.PlayCinematic(levelOutroCinematic, LoadNextLevel);
    }

    public virtual void LevelLost()
    {
        TimeManager.SetPausedTime();
        OnLevelLost?.Invoke(this, true);
    }

    public virtual void UndoLevelLost()
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
