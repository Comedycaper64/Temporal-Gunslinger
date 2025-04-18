using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool bLevelActive;

    [SerializeField]
    private bool bPlayIntroCinematicOnStart = true;

    public static GameManager Instance { get; private set; }
    public static EventHandler<StateEnum> OnGameStateChange;

    private static Transform revenantTransform;

    [SerializeField]
    protected CinemachineVirtualCamera endOfLevelCam;

    [SerializeField]
    protected RewindManager rewindManager;

    [SerializeField]
    protected CinematicSO levelIntroCinematic;

    [SerializeField]
    protected CinematicSO levelOutroCinematic;
    protected Coroutine windDownCoroutine;
    public event EventHandler<bool> OnLevelLost;
    public event EventHandler<bool> OnNoBulletsLeft;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        revenantTransform = GameObject
            .FindGameObjectWithTag("Player")
            ?.GetComponent<PlayerStateMachine>()
            ?.GetRevenantChest();

        if (revenantTransform == null)
        {
            revenantTransform = new GameObject("Rev Target").GetComponent<Transform>();
        }

        bLevelActive = false;

        TimeManager.ResetTimeManager();
    }

    public virtual void Start()
    {
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        rewindManager.OnResetLevel += RewindManager_OnResetLevel;
        rewindManager.OnRewindToStart += RewindManager_OnRewindToStart;

        SaveManager.SetLevelProgress(SceneManager.GetActiveScene().buildIndex);

        if (bPlayIntroCinematicOnStart)
        {
            CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
        }
    }

    protected virtual void OnDisable()
    {
        rewindManager.OnResetLevel -= RewindManager_OnResetLevel;
        rewindManager.OnRewindToStart -= RewindManager_OnRewindToStart;

        if (windDownCoroutine != null)
        {
            StopCoroutine(windDownCoroutine);
        }

        EnemyDeadState.enemiesAlive = 0;
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

    protected IEnumerator ResetLevel()
    {
        rewindManager.ResetManager(false);
        RedirectManager.Instance.ResetLevel();
        OnGameStateChange?.Invoke(this, StateEnum.idle);
        yield return null;

        TimeManager.UnpauseTime();
        RewindableMovement.UpdateMovementTimescale(1f);
    }

    protected virtual void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 25)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public virtual void EndLevel(Transform lastEnemy)
    {
        if (!bLevelActive)
        {
            return;
        }

        bLevelActive = false;
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        rewindManager.ToggleCanRewind(false);
        TimeManager.SetNormalTime();
        endOfLevelCam.gameObject.SetActive(true);
        endOfLevelCam.m_Follow = lastEnemy;
        endOfLevelCam.m_LookAt = lastEnemy;
        //switch on last enemy killed camera
        // do a focus on it
        windDownCoroutine = StartCoroutine(EndOfLevelWindDown());
    }

    public virtual IEnumerator EndOfLevelWindDown()
    {
        yield return new WaitForSeconds(2f);
        //Set rewindable movement timescale to be normal
        //RewindableMovement.UpdateMovementTimescale(1f);

        //MIGHT BREAK THINGS HOPEFULLY NOT
        rewindManager.ResetManager(true);
        endOfLevelCam.gameObject.SetActive(false);
        CinematicManager.Instance.PlayCinematic(levelOutroCinematic, LoadNextLevel);
    }

    public virtual void LevelLost(bool bulletsSpent = false)
    {
        if (!bLevelActive)
        {
            return;
        }

        TimeManager.SetPausedTime();
        endOfLevelCam.gameObject.SetActive(true);
        endOfLevelCam.m_Follow = revenantTransform;
        endOfLevelCam.m_LookAt = revenantTransform;
        OnLevelLost?.Invoke(this, true);
        OnNoBulletsLeft?.Invoke(this, bulletsSpent);
    }

    public virtual void UndoLevelLost()
    {
        endOfLevelCam.gameObject.SetActive(false);
        endOfLevelCam.m_Follow = null;
        endOfLevelCam.m_LookAt = null;
        OnLevelLost?.Invoke(this, false);
        OnNoBulletsLeft?.Invoke(this, false);
    }

    public bool IsLevelActive()
    {
        return bLevelActive;
    }

    public void PlayIntroCinematic()
    {
        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
    }

    public static Transform GetRevenant()
    {
        return revenantTransform;
    }

    protected void RewindManager_OnResetLevel()
    {
        //Debug.Log("RESET!");
        if (!bLevelActive)
        {
            return;
        }

        StartCoroutine(ResetLevel());
    }

    protected void RewindManager_OnRewindToStart()
    {
        OnGameStateChange?.Invoke(this, StateEnum.idle);
    }
}
