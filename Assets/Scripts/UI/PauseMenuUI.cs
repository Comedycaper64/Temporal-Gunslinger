using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PauseMenuUI : MonoBehaviour
{
    public static bool pauseActive = false;
    private CursorLockMode lastLockMode;

    [SerializeField]
    private CanvasGroup pauseMenuGroup;

    private CanvasGroupFader currentActiveGroup;

    [SerializeField]
    private CanvasGroupFader pauseMenuFader;

    [SerializeField]
    private CanvasGroupFader controlsFader;

    [SerializeField]
    private CanvasGroupFader optionsFader;

    [SerializeField]
    private CanvasGroupFader restartConfirmFader;

    [SerializeField]
    private CanvasGroupFader mainMenuConfirmFader;

    [SerializeField]
    private CanvasGroupFader quitConfirmFader;

    public static EventHandler<bool> OnPauseToggled;

    private void Start()
    {
        pauseActive = false;

        pauseMenuGroup.interactable = false;
        pauseMenuGroup.blocksRaycasts = false;
        pauseMenuFader.SetCanvasGroupAlpha(0f);

        InputManager.Instance.OnPauseAction += TogglePauseMenu;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPauseAction -= TogglePauseMenu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        TimeManager.ToggleMenuTimePause(false);
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        TimeManager.ToggleMenuTimePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseGroup(CanvasGroupFader fader)
    {
        fader.ToggleFade(false);
        CanvasGroup currentGroup = fader.GetComponent<CanvasGroup>();
        currentGroup.interactable = false;
        currentGroup.blocksRaycasts = false;
    }

    public void OpenGroup(CanvasGroupFader newGroupFader)
    {
        if (currentActiveGroup)
        {
            CloseGroup(currentActiveGroup);
        }

        newGroupFader.ToggleFade(true);
        CanvasGroup newGroup = newGroupFader.GetComponent<CanvasGroup>();
        newGroup.interactable = true;
        newGroup.blocksRaycasts = true;

        currentActiveGroup = newGroupFader;
    }

    public void TogglePauseMenu()
    {
        pauseActive = !pauseActive;

        pauseMenuGroup.interactable = pauseActive;
        pauseMenuGroup.blocksRaycasts = pauseActive;
        pauseMenuFader.ToggleFade(pauseActive);

        CloseGroup(controlsFader);
        CloseGroup(restartConfirmFader);
        CloseGroup(optionsFader);
        CloseGroup(mainMenuConfirmFader);
        CloseGroup(quitConfirmFader);

        if (pauseActive)
        {
            lastLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = lastLockMode;
        }

        TimeManager.ToggleMenuTimePause(pauseActive);

        OnPauseToggled?.Invoke(this, pauseActive);
    }
}
