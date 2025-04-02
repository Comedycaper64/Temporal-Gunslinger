using Steamworks;
using UnityEngine;

public class FamineBossACH : MonoBehaviour
{
    private int locustThreshold = 4;
    private int locustsSpawned = 0;

    [SerializeField]
    private RepeatingBulletSpawner bulletSpawner;
    private const string EFF_ACH = "ACH_EFFICIENT";

    private void OnEnable()
    {
        bulletSpawner.OnBulletSpawned += UpdateLocustsSpawned;
        FlameCarrier.OnLevelFinished += EvaluateAchievement;
    }

    private void OnDisable()
    {
        bulletSpawner.OnBulletSpawned -= UpdateLocustsSpawned;
        FlameCarrier.OnLevelFinished -= EvaluateAchievement;
    }

    private void UpdateLocustsSpawned(object sender, int spawned)
    {
        locustsSpawned = spawned;
    }

    private void EvaluateAchievement()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        //Debug.Log("Locusts Spawned: " + locustsSpawned);

        if (locustsSpawned < locustThreshold)
        {
            string achString = EFF_ACH;

            SteamUserStats.GetAchievement(achString, out bool achCompleted);

            if (!achCompleted)
            {
                SteamUserStats.SetAchievement(achString);
                SteamUserStats.StoreStats();
            }
        }
    }
}
