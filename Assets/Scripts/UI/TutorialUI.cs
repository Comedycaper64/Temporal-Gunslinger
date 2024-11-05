using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private bool displayTutorial = true;

    private bool tutorialDisplayed = false;

    [SerializeField]
    private CanvasGroupFader tutorialCanvas;

    [SerializeField]
    private VideoPlayer tutorialVideoPlayer;
    public static EventHandler<bool> OnDisplayTutorial;

    private void Awake()
    {
        if (displayTutorial)
        {
            GameManager.OnGameStateChange += GameManager_OnGameStateChange;
        }

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

        ToggleCanvas(true);
        OnDisplayTutorial?.Invoke(this, true);
        tutorialDisplayed = true;
    }

    public void ToggleCanvas(bool toggle)
    {
        tutorialCanvas.ToggleFade(toggle);
        tutorialCanvas.ToggleBlockRaycasts(toggle);
        if (tutorialVideoPlayer)
        {
            if (toggle)
            {
                tutorialVideoPlayer.Play();
            }
            else
            {
                tutorialVideoPlayer.Stop();
            }
        }
    }

    public void CloseTutorial()
    {
        ToggleCanvas(false);
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
