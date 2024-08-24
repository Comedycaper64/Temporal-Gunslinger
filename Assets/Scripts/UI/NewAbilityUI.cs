using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class NewAbilityUI : MonoBehaviour
{
    [SerializeField]
    private MMF_Player pulseFeedback;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += PulseAbility;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= PulseAbility;
    }

    private void PulseAbility(object sender, StateEnum state)
    {
        if (state == StateEnum.idle)
        {
            pulseFeedback.PlayFeedbacks();
        }
    }
}
