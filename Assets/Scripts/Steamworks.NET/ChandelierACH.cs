using Steamworks;
using UnityEngine;

public class ChandelierACH : MonoBehaviour
{
    private const string CHN_ACH = "ACH_CHAN";

    private BulletMovement chandelierMovement;

    private void OnEnable()
    {
        chandelierMovement = GetComponent<BulletMovement>();
        chandelierMovement.OnRedirect += EvaluateAchievement;
    }

    private void OnDisable()
    {
        chandelierMovement.OnRedirect -= EvaluateAchievement;
    }

    private void EvaluateAchievement()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        string achString = CHN_ACH;

        SteamUserStats.GetAchievement(achString, out bool achCompleted);

        if (!achCompleted)
        {
            SteamUserStats.SetAchievement(achString);
            SteamUserStats.StoreStats();
        }
    }
}
