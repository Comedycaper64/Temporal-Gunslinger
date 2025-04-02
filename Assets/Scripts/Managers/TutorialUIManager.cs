using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{
    private int openTutorialIndex = 0;
    private TutorialUI openTutorial;

    [SerializeField]
    private Button tutorialButton;

    [SerializeField]
    private TextMeshProUGUI tutorialText;

    [SerializeField]
    private List<TutorialUI> tutorialSlides = new List<TutorialUI>();

    private void OnEnable()
    {
        if (tutorialSlides.Count == 0)
        {
            tutorialButton.interactable = false;
        }
        else
        {
            openTutorial = tutorialSlides[0];
            openTutorialIndex = 0;
            UpdateTutorialNumberText();
        }
    }

    private void UpdateTutorialNumberText()
    {
        tutorialText.text = openTutorialIndex + 1 + " / " + tutorialSlides.Count;
    }

    private int CustomMod(float a, float b)
    {
        return (int)(a - b * Mathf.Floor(a / b));
    }

    public void OpenTutorial()
    {
        openTutorial.ToggleCanvas(true);
    }

    public void CloseTutorial()
    {
        openTutorial.ToggleCanvas(false);
    }

    public void NextTutorial()
    {
        openTutorialIndex = CustomMod(openTutorialIndex + 1, tutorialSlides.Count);
        openTutorial.ToggleCanvas(false);
        openTutorial = tutorialSlides[openTutorialIndex];
        openTutorial.ToggleCanvas(true);

        UpdateTutorialNumberText();
    }

    public void PreviousTutorial()
    {
        openTutorialIndex = CustomMod(openTutorialIndex - 1, tutorialSlides.Count);
        //Debug.Log(openTutorialIndex);
        openTutorial.ToggleCanvas(false);
        openTutorial = tutorialSlides[openTutorialIndex];
        openTutorial.ToggleCanvas(true);

        UpdateTutorialNumberText();
    }
}
