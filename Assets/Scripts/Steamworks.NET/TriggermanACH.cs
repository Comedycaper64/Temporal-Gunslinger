using Steamworks;
using UnityEngine;

public class TriggermanACH : MonoBehaviour
{
    private bool daggerSpawned = false;

    private const string TGR_ACH = "ACH_TRIGGER";

    private void OnEnable()
    {
        PlayerConquestAbility.OnConquestAbility += DaggerSpawned;
        GameManager.OnGameStateChange += UnspawnDagger;
        EnemyDeadState.OnEnemyDeadACH += EvaluateAchievement;
    }

    private void OnDisable()
    {
        PlayerConquestAbility.OnConquestAbility -= DaggerSpawned;
        GameManager.OnGameStateChange -= UnspawnDagger;
        EnemyDeadState.OnEnemyDeadACH -= EvaluateAchievement;
    }

    private void UnspawnDagger(object sender, StateEnum e)
    {
        daggerSpawned = false;
    }

    private void DaggerSpawned(object sender, CutInType e)
    {
        daggerSpawned = true;
    }

    private void EvaluateAchievement(object sender, int enemies)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (!daggerSpawned)
        {
            string achString = TGR_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
