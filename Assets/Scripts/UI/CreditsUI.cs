using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    [SerializeField]
    CanvasGroupFader canvasGroup;

    private void OnEnable()
    {
        EndingChoiceUI.OnGameBeat += StartCredits;
    }

    private void OnDisable()
    {
        EndingChoiceUI.OnGameBeat -= StartCredits;
    }

    private void StartCredits()
    {
        canvasGroup.ToggleFade(true);
        canvasGroup.ToggleBlockRaycasts(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
