using UnityEngine;

public class DefianceMenuUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader startGameFader;

    [SerializeField]
    private IntroPocketwatch introPocketwatch;

    [SerializeField]
    private CanvasGroup mainMenuGroup;

    public void ToggleStartGameFader(bool toggle)
    {
        startGameFader.ToggleFade(toggle);
        CanvasGroup newGroup = startGameFader.GetComponent<CanvasGroup>();
        newGroup.interactable = toggle;
        newGroup.blocksRaycasts = toggle;
    }

    public void StartNewGame()
    {
        ToggleStartGameFader(false);

        introPocketwatch.StopFeedbackPlayer();
        mainMenuGroup.interactable = false;
    }
}
