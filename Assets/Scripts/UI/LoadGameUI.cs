using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> levelBackgrounds = new List<Image>();

    [SerializeField]
    private List<Button> levelButtons = new List<Button>();

    private void Awake()
    {
        DisableButtons();

        int levelProgress = SaveManager.GetLevelProgress();

        ShowBackgrounds(levelProgress);

        if (levelProgress == 0)
        {
            levelButtons[0].interactable = false;
            return;
        }

        for (int i = 1; i <= levelProgress; i++)
        {
            if (i >= levelButtons.Count)
            {
                break;
            }

            levelButtons[i].gameObject.SetActive(true);
        }
    }

    private void ShowBackgrounds(int levelProgress)
    {
        foreach (Image bg in levelBackgrounds)
        {
            bg.enabled = false;
        }

        if (levelProgress > 0)
        {
            levelBackgrounds[0].enabled = true;
        }

        if (levelProgress > 6)
        {
            levelBackgrounds[1].enabled = true;
        }

        if (levelProgress > 12)
        {
            levelBackgrounds[2].enabled = true;
        }

        if (levelProgress > 18)
        {
            levelBackgrounds[3].enabled = true;
        }
    }

    private void DisableButtons()
    {
        foreach (Button levelButton in levelButtons)
        {
            if (levelButton == levelButtons[0])
            {
                continue;
            }

            levelButton.gameObject.SetActive(false);
        }
    }
}
