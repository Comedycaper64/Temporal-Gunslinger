using System;
using System.Collections;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private bool tutorialDisplayed = false;

    [SerializeField]
    private CanvasGroupFader tutorialCanvas;
    public static EventHandler<bool> OnDisplayTutorial;

    private void Awake()
    {
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
        tutorialCanvas.SetCanvasGroupAlpha(0f);
        tutorialCanvas.ToggleBlockRaycasts(false);
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
    }

    private IEnumerator DisplayTutorial()
    {
        yield return null;

        tutorialCanvas.ToggleFade(true);
        tutorialCanvas.ToggleBlockRaycasts(true);
        OnDisplayTutorial?.Invoke(this, true);
        // turn off revenant controls
        //Make cursor appear
        tutorialDisplayed = true;
    }

    public void CloseTutorial()
    {
        tutorialCanvas.ToggleFade(false);
        tutorialCanvas.ToggleBlockRaycasts(false);
        OnDisplayTutorial?.Invoke(this, false);
    }

    private void GameManager_OnGameStateChange(object sender, StateEnum state)
    {
        if (state == StateEnum.idle)
        {
            if (!tutorialDisplayed)
            {
                StartCoroutine(DisplayTutorial());
            }
        }
    }
}
