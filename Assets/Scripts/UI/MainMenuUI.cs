using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
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
    private CanvasGroupFader loadGameFader;

    [SerializeField]
    private CanvasGroupFader optionsFader;

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
                continueText += " - Medieval Level 5";
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
                continueText += " - Western Level 5";
                break;
            case 12:
                continueText += " - Western Intermission";
                break;
            case 13:
                continueText += " - Modern Level 1";
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
        SceneManager.LoadScene(SaveManager.GetLevelProgress());
    }

    public void LoadGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}