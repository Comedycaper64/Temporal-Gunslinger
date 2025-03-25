using Steamworks;
using UnityEngine;

public class CrystalACH : MonoBehaviour
{
    private const string PNT_ACH = "ACH_PLANT";

    private void OnEnable()
    {
        CrystalDeadState.OnCrystalDeadACH += EvaluateAchievement;
    }

    private void OnDisable()
    {
        CrystalDeadState.OnCrystalDeadACH -= EvaluateAchievement;
    }

    private void EvaluateAchievement()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        string achString = PNT_ACH;

        SteamUserStats.GetAchievement(achString, out bool achCompleted);

        if (!achCompleted)
        {
            SteamUserStats.SetAchievement(achString);
            SteamUserStats.StoreStats();
        }
    }
}
