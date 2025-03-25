using System;
using Steamworks;
using UnityEngine;

public class DeathBossACH : MonoBehaviour
{
    private int coinThreshold = 6;
    private const string GDS_ACH = "ACH_GOODSHOW";

    private void OnEnable()
    {
        EnemyBossDeadState.OnBossDeadACH += EvaluateAchievement;
    }

    private void OnDisable()
    {
        EnemyBossDeadState.OnBossDeadACH -= EvaluateAchievement;
    }

    private void EvaluateAchievement()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        int remainingCoins = RedirectManager.Instance.GetRemainingRedirects();

        //Debug.Log("Remaining coins: " + remainingCoins);

        if (remainingCoins >= coinThreshold)
        {
            string achString = GDS_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
