using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DangerTracker : MonoBehaviour
{
    private PocketwatchUI pocketwatchUI;
    public static Dictionary<RewindableMovement, float> dangers =
        new Dictionary<RewindableMovement, float>();

    private void Awake()
    {
        pocketwatchUI = GetComponent<PocketwatchUI>();
        EnemyMovement.OnEnemyMovementChange += GetDeathTime;
        BulletMovement.OnChangeDirection += SetNewDeathTime;
        pocketwatchUI.OnShowUI += GetDeathTime;
    }

    private void SetNewDeathTime(object sender, float newDeathTime)
    {
        RewindableMovement movement = sender as RewindableMovement;

        dangers[movement] = pocketwatchUI.GetCurrentPocketwatchTime() + newDeathTime;

        GetDeathTime();
    }

    private void GetDeathTime()
    {
        //Get lowest death time
        //Set it in pocketweatch ui

        float lowestDeathTime = 9999f;

        List<float> deathTimes = dangers.Values.ToList();

        foreach (float deathTime in deathTimes)
        {
            if (deathTime < 0f)
            {
                continue;
            }

            if (deathTime < lowestDeathTime)
            {
                lowestDeathTime = deathTime;
            }
        }

        if (lowestDeathTime < 9999f)
        {
            //Debug.Log("Closest object time: " + lowestDeathTime);
            pocketwatchUI.SetDeathTime(lowestDeathTime);
        }
        else
        {
            pocketwatchUI.SetDeathTime(-1f);
        }
    }
}
