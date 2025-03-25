using Steamworks;
using UnityEngine;

public class PestilenceAssistACH : MonoBehaviour
{
    private const string ALC_ACH = "ACH_ALACRITY";

    private void OnEnable()
    {
        BulletBooster.OnBoostACH += EvaluateAchievement;
    }

    private void OnDisable()
    {
        BulletBooster.OnBoostACH -= EvaluateAchievement;
    }

    private void EvaluateAchievement()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        string achString = ALC_ACH;

        SteamUserStats.GetAchievement(achString, out bool achCompleted);

        if (!achCompleted)
        {
            SteamUserStats.SetAchievement(achString);
            SteamUserStats.StoreStats();
        }
    }
}
