using Steamworks;
using UnityEngine;

public class EndingACH : MonoBehaviour
{
    private const string DEFY_ACH = "ACH_DEFY";
    private const string SUB_ACH = "ACH_SUB";

    public void EvaluateAchievements(bool defy)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (defy)
        {
            SteamUserStats.GetAchievement(DEFY_ACH, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(DEFY_ACH);
                SteamUserStats.StoreStats();
            }
        }
        else
        {
            SteamUserStats.GetAchievement(SUB_ACH, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(SUB_ACH);
                SteamUserStats.StoreStats();
            }
        }
    }
}
