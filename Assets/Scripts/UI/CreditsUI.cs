using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    private int openCreditIndex = 0;
    private CanvasGroupFader openCredit;

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
