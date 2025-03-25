using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    private int openCreditIndex = 0;
    private CanvasGroupFader openCredit;
    private EndingACH endingAchievements;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private List<CanvasGroupFader> creditSlides = new List<CanvasGroupFader>();

    [SerializeField]
    private CanvasGroupFader canvasGroup;

    private void OnEnable()
    {
        EndingChoiceUI.OnGameBeat += StartCredits;
        DefianceGameManager.OnGameBeat += StartCredits;

        endingAchievements = GetComponent<EndingACH>();

        openCreditIndex = 0;
        openCredit = creditSlides[0];
    }

    private void OnDisable()
    {
        EndingChoiceUI.OnGameBeat -= StartCredits;
        DefianceGameManager.OnGameBeat -= StartCredits;
    }

    private void StartCredits()
    {
        canvasGroup.ToggleFade(true);
        canvasGroup.ToggleBlockRaycasts(true);

        if (SceneManager.GetActiveScene().buildIndex > 24)
        {
            endingAchievements.EvaluateAchievements(true);
        }
        else
        {
            endingAchievements.EvaluateAchievements(false);
        }
    }

    public void NextTutorial()
    {
        openCreditIndex++;
        openCredit.ToggleFade(false);
        openCredit.ToggleBlockRaycasts(false);
        openCredit = creditSlides[openCreditIndex];
        openCredit.ToggleBlockRaycasts(true);
        openCredit.ToggleFade(true);

        if (openCreditIndex >= (creditSlides.Count - 1))
        {
            closeButton.gameObject.SetActive(true);
        }
    }

    public void PreviousTutorial()
    {
        openCreditIndex--;
        openCredit.ToggleFade(false);
        openCredit.ToggleBlockRaycasts(false);
        openCredit = creditSlides[openCreditIndex];
        openCredit.ToggleBlockRaycasts(true);
        openCredit.ToggleFade(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
