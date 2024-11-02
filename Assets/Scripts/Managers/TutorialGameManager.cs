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
    private GameObject tutorialUI;

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

    private TutorialState tutorialState;

    public override void Start()
    {
        tutorialState = TutorialState.MainMenu;
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        //playerStateMachine.stateMachineAnimator.CrossFadeInFixedTime("Revenant walk", 0.1f);
        playerController = playerStateMachine.GetComponent<PlayerController>();
        playerController.ToggleTutorialStartMode();
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
        rewindManager.ToggleCanRewind(false);
    }

    public void StartGame()
    {
        startOfLevelCam.gameObject.SetActive(false);
        pauseMenuUI.SetActive(true);
        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
        EnemyDeadState.enemiesAlive = 1;
    }

    public void LoadGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public override void SetupLevel()
    {
        base.SetupLevel();
        tutorialState++;
    }

    public override void LevelLost()
    {
        base.LevelLost();
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
            revPocketwatch.SetActive(true);
            revModelPocketwatch.SetActive(true);
            pocketwatchUI.SetActive(true);
            tutorialUI.SetActive(true);
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
