using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class PauseMenuUI : MonoBehaviour
{
    private bool listeningToInput = false;
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
    private CanvasGroupFader tutorialFader;

    [SerializeField]
    private TutorialUIManager tutorialManager;

    [SerializeField]
    private CanvasGroupFader restartConfirmFader;

    [SerializeField]
    private CanvasGroupFader skipConfirmFader;

    [SerializeField]
    private CanvasGroupFader mainMenuConfirmFader;

    [SerializeField]
    private CanvasGroupFader quitConfirmFader;

    [SerializeField]
    private Button skipCutsceneButton;

    public static EventHandler<bool> OnPauseToggled;
    public static Action OnSkipCutscene;
    public static Action OnExitLevel;

    private void Start()
    {
        pauseActive = false;

        pauseMenuGroup.interactable = false;
        pauseMenuGroup.blocksRaycasts = false;
        pauseMenuFader.SetCanvasGroupAlpha(0f);

        if (!listeningToInput)
        {
            InputManager.Instance.OnPauseAction += TogglePauseMenu;
            listeningToInput = true;
        }
    }

    private void OnEnable()
    {
        if (!listeningToInput && InputManager.Instance)
        {
            InputManager.Instance.OnPauseAction += TogglePauseMenu;
            listeningToInput = true;
        }
        GameManager.OnGameStateChange += ToggleCanSkipCutscene;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPauseAction -= TogglePauseMenu;
        listeningToInput = false;
        GameManager.OnGameStateChange -= ToggleCanSkipCutscene;
    }

    public void QuitGame()
    {
        OnExitLevel?.Invoke();
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        TimeManager.ToggleMenuTimePause(false);
        OnExitLevel?.Invoke();
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

        currentActiveGroup = null;
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

    public void TrySkipCinematic()
    {
        TogglePauseMenu();
        OnSkipCutscene?.Invoke();
    }

    private void ToggleCanSkipCutscene(object sender, StateEnum state)
    {
        if (state == StateEnum.idle)
        {
            skipCutsceneButton.interactable = false;
        }
        else if (state == StateEnum.inactive)
        {
            skipCutsceneButton.interactable = true;
        }
    }

    public void TogglePauseMenu()
    {
        pauseActive = !pauseActive;

        pauseMenuGroup.interactable = pauseActive;
        pauseMenuGroup.blocksRaycasts = pauseActive;
        pauseMenuFader.ToggleFade(pauseActive);

        CloseGroup(controlsFader);
        CloseGroup(restartConfirmFader);
        CloseGroup(skipConfirmFader);
        CloseGroup(optionsFader);
        CloseGroup(tutorialFader);
        CloseGroup(mainMenuConfirmFader);
        CloseGroup(quitConfirmFader);

        tutorialManager.CloseTutorial();

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
