using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class DefianceGameManager : GameManager
{
    [SerializeField]
    private GameObject reaper;

    [SerializeField]
    private GameObject revPocketwatch;

    [SerializeField]
    private GameObject revModelPocketwatch;

    [SerializeField]
    private GameObject pocketwatchUI;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject vfxCleanup;

    [SerializeField]
    private GameObject[] abilityUI;

    [SerializeField]
    private PlayerStateMachine playerStateMachine;
    private PlayerController playerController;

    [SerializeField]
    private CinemachineVirtualCamera startOfLevelCam;

    [SerializeField]
    private CinematicSO defianceIntroCinematic;

    [SerializeField]
    private CinematicSO defianceMidPointCinematic;

    [SerializeField]
    private CinematicSO defianceOutroCinematic;

    private TutorialState tutorialState;

    public static Action OnGameBeat;

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

        rewindManager.OnResetLevel += RewindManager_OnResetLevel;
        rewindManager.OnRewindToStart += RewindManager_OnRewindToStart;
        EnemyReaperMaskDeadState.OnReaperMaskKilled += AdvanceLevel;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyReaperMaskDeadState.OnReaperMaskKilled -= AdvanceLevel;

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

    public override void LevelLost()
    {
        base.LevelLost();
    }

    private void AdvanceLevel(object sender, Transform lastEnemy)
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
        StartCoroutine(EndOfLevelWindDown());
    }

    private void ShowCredits()
    {
        OnGameBeat?.Invoke();
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

            CinematicManager.Instance.PlayCinematic(defianceIntroCinematic, SetupLevel);
            //round2Mask1.SetActive(true);
            //round2Mask2.SetActive(true);
            EnemyDeadState.enemiesAlive = 3;
            //BulletDeadState.bulletNumber = 1;
            playerController.ToggleCanRotate(true);
            playerController.ToggleCanFocus(true);
            playerController.ToggleCanRedirect(true);
            playerController.ToggleCanPossess(true);
            rewindManager.ToggleCanRewind(true);

            foreach (GameObject ui in abilityUI)
            {
                ui.SetActive(true);
            }
        }
        else if (tutorialState == TutorialState.Round2)
        {
            CinematicManager.Instance.PlayCinematic(defianceMidPointCinematic, SetupLevel);
            EnemyDeadState.enemiesAlive = 99;
            RedirectManager.Instance.SetRedirects(0);
            playerStateMachine.GetComponent<PlayerConquestAbility>().bCanUseAbility = true;
            //BulletDeadState.bulletNumber = 2;

            rewindManager.ToggleCanRewind(true);
        }
        else if (tutorialState == TutorialState.Round3)
        {
            EnemyDeadState.enemiesAlive = 0;
            BulletDeadState.bulletNumber = 0;
            CinematicManager.Instance.PlayCinematic(defianceOutroCinematic, ShowCredits);
        }
    }
}
