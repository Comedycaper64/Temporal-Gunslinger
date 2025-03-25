using Steamworks;
using UnityEngine;

public class KnightACH : MonoBehaviour
{
    private const string KNG_TAG = "Knight";
    private const string KNG_ACH = "ACH_KNIGHT";

    private BulletDamager damager;

    private void OnEnable()
    {
        damager = GetComponent<BulletDamager>();

        damager.OnHitAchievementCheck += EvaluateAchievement;
    }

    private void OnDisable()
    {
        damager.OnHitAchievementCheck -= EvaluateAchievement;
    }

    private void EvaluateAchievement(object sender, IDamageable damageable)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        WeakPoint weakPoint = damageable as WeakPoint;

        if (!weakPoint)
        {
            return;
        }

        if (weakPoint.gameObject.CompareTag(KNG_TAG))
        {
            string achString = KNG_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
