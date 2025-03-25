using Steamworks;
using UnityEngine;

public class WarAssistACH : MonoBehaviour
{
    private BulletPossessTarget bulletPossessTarget;
    private const string CLV_ACH = "ACH_CLEVER";
    private const string FRLK_TAG = "Freelook";

    private void OnEnable()
    {
        BulletPossessor.OnNewBulletPossessed += UpdatePossessedBullet;
        PlayerConquestAbility.OnConquestAbility += EvaluateAchievement;
    }

    private void OnDisable()
    {
        BulletPossessor.OnNewBulletPossessed -= UpdatePossessedBullet;
        PlayerConquestAbility.OnConquestAbility -= EvaluateAchievement;
    }

    private void UpdatePossessedBullet(object sender, BulletPossessTarget newTarget)
    {
        bulletPossessTarget = newTarget;
    }

    private void EvaluateAchievement(object sender, CutInType e)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (bulletPossessTarget.gameObject.CompareTag(FRLK_TAG))
        {
            string achString = CLV_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
