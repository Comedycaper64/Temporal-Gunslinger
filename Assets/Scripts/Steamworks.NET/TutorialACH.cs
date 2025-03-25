using Steamworks;
using UnityEngine;

public class TutorialACH : MonoBehaviour
{
    private const string TUT_ACH = "ACH_TUTORIAL_REAPER";

    private void OnEnable()
    {
        EnemyReaperMaskDeadState.OnReaperMaskKilled += EvaluateAchievement;
    }

    private void OnDisable()
    {
        EnemyReaperMaskDeadState.OnReaperMaskKilled -= EvaluateAchievement;
    }

    private void EvaluateAchievement(object sender, Transform e)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        SteamUserStats.GetAchievement(TUT_ACH, out bool achCompleted);

        if (!achCompleted)
        {
            SteamUserStats.SetAchievement(TUT_ACH);
            SteamUserStats.StoreStats();
        }
    }
}
