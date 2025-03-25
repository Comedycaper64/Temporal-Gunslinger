using Steamworks;
using UnityEngine;

public class SelfKillACH : MonoBehaviour
{
    private const string REV_TAG = "Revenant";
    private const string MIS_ACH = "ACH_MISCAL";
    private const string BET_ACH = "ACH_BETRAY";

    [SerializeField]
    private bool warAbility = false;
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

        if (weakPoint.gameObject.CompareTag(REV_TAG))
        {
            string achString = MIS_ACH;

            if (warAbility)
            {
                achString = BET_ACH;
            }

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
