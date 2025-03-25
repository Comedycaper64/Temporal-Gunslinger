using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressACH : MonoBehaviour
{
    private const string WAR_ACH = "ACH_WAR";
    private const string FAM_ACH = "ACH_FAMINE";
    private const string PES_ACH = "ACH_PESTILENCE";
    private const string DTH_ACH = "ACH_DEATH";

    private void Start()
    {
        EvaluateAchievements();
    }

    private void EvaluateAchievements()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex > 5)
        {
            SteamUserStats.GetAchievement(WAR_ACH, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(WAR_ACH);
                SteamUserStats.StoreStats();
            }
        }

        if (buildIndex > 11)
        {
            SteamUserStats.GetAchievement(FAM_ACH, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(FAM_ACH);
                SteamUserStats.StoreStats();
            }
        }

        if (buildIndex > 17)
        {
            SteamUserStats.GetAchievement(PES_ACH, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(PES_ACH);
                SteamUserStats.StoreStats();
            }
        }

        if (buildIndex > 23)
        {
            SteamUserStats.GetAchievement(DTH_ACH, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(DTH_ACH);
                SteamUserStats.StoreStats();
            }
        }
    }
}
