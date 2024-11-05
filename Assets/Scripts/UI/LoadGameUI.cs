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

        if (levelProgress > 7)
        {
            levelBackgrounds[1].enabled = true;
        }

        if (levelProgress > 13)
        {
            levelBackgrounds[2].enabled = true;
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
