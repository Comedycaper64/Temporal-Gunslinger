using Steamworks;
using UnityEngine;

public class KillCountACH : MonoBehaviour
{
    private const string BLD_ACH = "ACH_BLOOD";

    private void OnEnable()
    {
        EnemyDeadState.OnEnemyDeadACH += EvaluateAchievement;
    }

    private void OnDisable()
    {
        EnemyDeadState.OnEnemyDeadACH -= EvaluateAchievement;
    }

    private void EvaluateAchievement(object sender, int enemiesAlive)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (enemiesAlive == 1)
        {
            string achString = BLD_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
