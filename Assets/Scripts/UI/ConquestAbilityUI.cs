using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class ConquestAbilityUI : MonoBehaviour
{
    [SerializeField]
    private MMF_Player usedFeedback;

    [SerializeField]
    private MMF_Player regenFeedback;

    [SerializeField]
    private Image abilityImage;

    [SerializeField]
    private CanvasGroupFader canvasFader;

    private void Awake()
    {
        canvasFader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += ToggleUI;
        PlayerConquestAbility.OnAbilityUIUsed += ToggleAbilityUsed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleUI;
        PlayerConquestAbility.OnAbilityUIUsed -= ToggleAbilityUsed;
    }

    private void ToggleAbilityUsed(object sender, bool toggle)
    {
        if (toggle)
        {
            usedFeedback.PlayFeedbacks();
        }
        else
        {
            regenFeedback.PlayFeedbacks();
        }
    }

    private void ToggleUI(object sender, StateEnum state)
    {
        if (state == StateEnum.active)
        {
            canvasFader.ToggleFade(true);
        }
        else
        {
            canvasFader.ToggleFade(false);
        }
    }
}
