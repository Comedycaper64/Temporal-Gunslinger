using Steamworks;
using UnityEngine;

public class PestilenceBossACH : MonoBehaviour
{
    private int livingEnemies = 99;
    private const string THO_ACH = "ACH_HUSK";

    private void OnEnable()
    {
        EnemyDeadState.OnEnemyDeadACH += UpdateLivingEnemies;
        EnemyBossDeadState.OnBossDeadACH += EvaluateAchievement;
    }

    private void OnDisable()
    {
        EnemyDeadState.OnEnemyDeadACH -= UpdateLivingEnemies;
        EnemyBossDeadState.OnBossDeadACH -= EvaluateAchievement;
    }

    private void UpdateLivingEnemies(object sender, int enemies)
    {
        livingEnemies = enemies;
    }

    private void EvaluateAchievement()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (livingEnemies <= 3)
        {
            string achString = THO_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
