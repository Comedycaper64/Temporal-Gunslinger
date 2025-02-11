using MoreMountains.Feedbacks;
using UnityEngine;

public class PestilenceAbilityUI : MonoBehaviour
{
    [SerializeField]
    private MMF_Player usedFeedback;

    [SerializeField]
    private CanvasGroupFader canvasFader;

    private void Awake()
    {
        canvasFader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += ToggleUI;
        PlayerPestilenceAbility.OnAbilityUIUsed += AbilityUsed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleUI;
        PlayerPestilenceAbility.OnAbilityUIUsed -= AbilityUsed;
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

    private void AbilityUsed()
    {
        usedFeedback.PlayFeedbacks();
    }
}
