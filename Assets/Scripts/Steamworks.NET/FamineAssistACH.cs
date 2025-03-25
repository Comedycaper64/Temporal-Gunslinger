using Steamworks;
using UnityEngine;

public class FamineAssistACH : MonoBehaviour
{
    private const string GRD_ACH = "ACH_GREED";

    private void OnEnable()
    {
        PlayerFamineAbility.OnFamineAbility += EvaluateAchievement;
    }

    private void OnDisable()
    {
        PlayerFamineAbility.OnFamineAbility -= EvaluateAchievement;
    }

    private void EvaluateAchievement(object sender, CutInType cutIn)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (BulletDeadState.bulletNumber <= 1)
        {
            string achString = GRD_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
