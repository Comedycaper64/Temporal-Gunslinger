using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private int loadBuildIndex = 0;
    private CanvasGroupFader currentActiveGroup;

    [SerializeField]
    private GameObject pauseOptionMenu;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private TextMeshProUGUI continueButtonText;

    [SerializeField]
    private IntroPocketwatch introPocketwatch;

    [SerializeField]
    private CanvasGroup mainMenuGroup;

    [SerializeField]
    private CanvasGroupFader startGameConfirmFader;

    [SerializeField]
    private GameObject savingGameUI;

    [SerializeField]
    private CanvasGroupFader optionsFader;

    [SerializeField]
    private CinematicSO levelLoadFade;

    private void Start()
    {
        if (SaveManager.GetLevelProgress() <= 0)
        {
            continueButton.interactable = false;
        }
        else
        {
            SetContinueButtonText();
        }
    }

    private void SetContinueButtonText()
    {
        string continueText = "Continue";

        switch (SaveManager.GetLevelProgress())
        {
            case 1:
                continueText += " - Medieval Level 1";
                break;
            case 2:
                continueText += " - Medieval Level 2";
                break;
            case 3:
                continueText += " - Medieval Level 3";
                break;
            case 4:
                continueText += " - Medieval Level 4";
                break;
            case 5:
                continueText += " - Medieval Boss ~ War";
                break;
            case 6:
                continueText += " - Medieval Intermission";
                break;
            case 7:
                continueText += " - Western Level 1";
                break;
            case 8:
                continueText += " - Western Level 2";
                break;
            case 9:
                continueText += " - Western Level 3";
                break;
            case 10:
                continueText += " - Western Level 4";
                break;
            case 11:
                continueText += " - Western Boss ~ Famine";
                break;
            case 12:
                continueText += " - Western Intermission";
                break;
            case 13:
                continueText += " - Modern Level 1";
                break;
            case 14:
                continueText += " - Modern Level 2";
                break;
            case 15:
                continueText += " - Modern Level 3";
                break;
            case 16:
                continueText += " - Modern Level 4";
                break;
            case 17:
                continueText += " - Modern Boss ~ Pestilence";
                break;
            case 18:
                continueText += " - Modern Intermission";
                break;
            case 19:
                continueText += " - Future Level 1";
                break;
            case 20:
                continueText += " - Future Level 2";
                break;
            case 21:
                continueText += " - Future Level 3";
                break;
            case 22:
                continueText += " - Future Level 4";
                break;
            case 23:
                continueText += " - Future ~ Death";
                break;
            case 24:
                continueText += " - Epilogue";
                break;
            default:
                break;
        }

        continueButtonText.text = continueText;
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

    public void StartNewGame()
    {
        CloseGroup(startGameConfirmFader);
        optionsFader.gameObject.SetActive(false);
        pauseOptionMenu.SetActive(true);

        introPocketwatch.StopFeedbackPlayer();
        mainMenuGroup.interactable = false;
    }

    public void ContinueGame()
    {
        loadBuildIndex = SaveManager.GetLevelProgress();
        CinematicManager.Instance.PlayCinematic(levelLoadFade, LoadLevel);
    }

    public void LoadGame(int buildIndex)
    {
        loadBuildIndex = buildIndex;
        savingGameUI.SetActive(false);
        mainMenuGroup.interactable = false;
        CinematicManager.Instance.PlayCinematic(levelLoadFade, LoadLevel);
    }

    private void LoadLevel()
    {
        SceneManager.LoadSceneAsync(loadBuildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
