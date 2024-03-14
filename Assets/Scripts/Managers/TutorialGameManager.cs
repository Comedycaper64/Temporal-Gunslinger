using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
    private GameObject round2Mask1;

    [SerializeField]
    private GameObject round2Mask2;

    [SerializeField]
    private GameObject revPocketwatch;

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

    private TutorialState tutorialState = TutorialState.MainMenu;

    public override void Start()
    {
        OnGameStateChange?.Invoke(this, StateEnum.inactive);
        playerStateMachine.stateMachineAnimator.CrossFadeInFixedTime("Revenant walk", 0.1f);
        playerController = playerStateMachine.GetComponent<PlayerController>();
        playerController.ToggleTutorialStartMode();
        rewindManager.ToggleCanReset(false);
        rewindManager.ToggleCanRewind(false);
    }

    public void StartGame()
    {
        startOfLevelCam.gameObject.SetActive(false);
        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, SetupLevel);
        EnemyDeadState.enemyNumber = 1;
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
        rewindManager.ResetManager();
        if (tutorialState == TutorialState.Round1)
        {
            reaper.SetActive(true);
            revPocketwatch.SetActive(true);
            CinematicManager.Instance.PlayCinematic(endOfRound1Cinematic, SetupLevel);
            //round2Mask1.SetActive(true);
            //round2Mask2.SetActive(true);
            EnemyDeadState.enemyNumber = 2;
            playerController.ToggleCanRotate(true);
            playerController.ToggleCanFocus(true);
            playerController.ToggleCanRedirect(true);
            rewindManager.ToggleCanRewind(true);
        }
        else if (tutorialState == TutorialState.Round2)
        {
            CinematicManager.Instance.PlayCinematic(endOfRound2Cinematic, SetupLevel);
            EnemyDeadState.enemyNumber = 2;
            playerController.ToggleCanPossess(true);
        }
        else if (tutorialState == TutorialState.Round3) { }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
