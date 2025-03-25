using Steamworks;
using UnityEngine;

public class AchievementResetter : MonoBehaviour
{
    private void Start()
    {
        SteamUserStats.ResetAllStats(true);
    }
}
