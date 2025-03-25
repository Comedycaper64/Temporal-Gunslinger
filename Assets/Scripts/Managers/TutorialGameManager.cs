using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

enum TutorialState
{
    MainMenu,
    Round1,
    Round2,
    Round3,
}

public class TutorialGameManager : GameManager
{
    private bool testBool = true;

    [SerializeField]
    private GameObject reaper;

    [SerializeField]
    private LevelSelectorUI levelSelectUI;

    [SerializeField]
    private GameObject revPocketwatch;

    [SerializeField]
    private GameObject revModelPocketwatch;

    [SerializeField]
    private GameObject pocketwatchUI;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject firstTutorialUI;

    [SerializeField]
    private GameObject secondTutorialUI;

    [SerializeField]
    private GameObject vfxCleanup;

    [SerializeField]
    private MaskStateMachine projectileMask;

    [SerializeField]
    private PlayerStateMachine playerStateMachine;
    private PlayerController playerController;

    [SerializeField]
    private CinemachineVirtualCamera startOfLevelCam;

    [SerializeField]
    private CinematicSO endOfRound1Cinematic;

    [SerializeField]
    private CinematicSO endOfRound2Cinematic;

    [SerializeField]
    private CinematicSO endOfRound3Cinematic;

    [SerializeField]
    private CinematicSO derailmentCinematic;

    private TutorialState tutorialState;

    public override void Start()
    {
        tutorialState = TutorialState.MainMenu;
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        //playerStateMachine.stateMachineAnimator.CrossFadeInFixedTime("Revenant walk", 0.1f);
        playerController = playerStateMachine.GetComponent<PlayerController>();
        if (testBool)
        {
            playerController.ToggleTutorialStartMode();
        }

        reaper.SetActive(false);

        if (revPocketwatch)
        {
            revPocketwatch.SetActive(false);
        }

        if (revModelPocketwatch)
        {
            revModelPocketwatch.SetActive(false);
        }

        if (pocketwatchUI)
        {
            pocketwatchUI.SetActive(false);
        }

        rewindManager.ToggleCanReset(false);

        if (testBool)
        {
            rewindManager.ToggleCanRewind(false);
        }

        rewindManager.OnResetLevel += RewindManager_OnResetLevel;
        rewindManager.OnRewindToStart += RewindManager_OnRewindToStart;
        EnemyReaperMaskDeadState.OnReaperMaskKilled += DerailTutorial;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyReaperMaskDeadState.OnReaperMaskKilled -= DerailTutorial;

        EnemyDeadState.enemiesAlive = 0;
        BulletDeadState.bulletNumber = 0;
    }

    public void StartGame()
    {
        startOfLevelCam.gameObject.SetActive(false);
        pauseMenuUI.SetActive(true);
        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
        EnemyDeadState.enemiesAlive = 1;
    }

    public override void SetupLevel()
    {
        base.SetupLevel();
        tutorialState++;
    }

    public override void LevelLost(bool bulletsSpent = false)
    {
        base.LevelLost();
    }

    public void EndTutorial()
    {
        CinematicManager.Instance.PlayCinematic(levelOutroCinematic, LoadNextLevel);
    }

    private void DerailTutorial(object sender, Transform lastEnemy)
    {
        bLevelActive = false;
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        rewindManager.ToggleCanRewind(false);
        TimeManager.SetNormalTime();
        endOfLevelCam.gameObject.SetActive(true);
        endOfLevelCam.m_Follow = lastEnemy;
        endOfLevelCam.m_LookAt = lastEnemy;
        //switch on last enemy killed camera
        // do a focus on it
        StartCoroutine(EndOfTutorialDerail());
    }

    private IEnumerator EndOfTutorialDerail()
    {
        yield return new WaitForSeconds(2f);
        RewindableMovement.UpdateMovementTimescale(1f);
        endOfLevelCam.gameObject.SetActive(false);
        rewindManager.ResetManager(true);

        CinematicManager.Instance.PlayCinematic(derailmentCinematic, RestartGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public override IEnumerator EndOfLevelWindDown()
    {
        yield return new WaitForSeconds(2f);
        RewindableMovement.UpdateMovementTimescale(1f);
        endOfLevelCam.gameObject.SetActive(false);
        rewindManager.ResetManager(true);
        if (tutorialState == TutorialState.Round1)
        {
            reaper.SetActive(true);
            vfxCleanup.SetActive(true);
            revPocketwatch.SetActive(true);
            revModelPocketwatch.SetActive(true);
            pocketwatchUI.SetActive(true);
            firstTutorialUI.SetActive(true);
            CinematicManager.Instance.PlayCinematic(endOfRound1Cinematic, SetupLevel);
            //round2Mask1.SetActive(true);
            //round2Mask2.SetActive(true);
            EnemyDeadState.enemiesAlive = 2;
            BulletDeadState.bulletNumber = 1;
            playerController.ToggleCanRotate(true);
            playerController.ToggleCanFocus(true);
            playerController.ToggleCanRedirect(true);
            rewindManager.ToggleCanRewind(true);
        }
        else if (tutorialState == TutorialState.Round2)
        {
            CinematicManager.Instance.PlayCinematic(endOfRound2Cinematic, SetupLevel);
            EnemyDeadState.enemiesAlive = 2;
            BulletDeadState.bulletNumber = 2;
            secondTutorialUI.SetActive(true);
            projectileMask.EnableFireProjectile();
            playerController.ToggleCanPossess(true);
            rewindManager.ToggleCanRewind(true);
        }
        else if (tutorialState == TutorialState.Round3)
        {
            EnemyDeadState.enemiesAlive = 0;
            BulletDeadState.bulletNumber = 0;
            CinematicManager.Instance.PlayCinematic(endOfRound3Cinematic, ShowLevelSelect);
        }
    }

    private void ShowLevelSelect()
    {
        levelSelectUI.ToggleLevelSelector(true);
    }
}
