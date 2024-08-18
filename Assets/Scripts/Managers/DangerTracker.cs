using System.Collections;
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

    //MAY NEED OPTIMISING: Locked on Bullets redirect every frame. Potentially make it so that it stores death times and only recalculates those that change?

    private void SetNewDeathTime(object sender, float newDeathTime)
    {
        RewindableMovement movement = sender as RewindableMovement;

        dangers[movement] = newDeathTime;

        GetDeathTime();

        // float lowestDeathTime = 9999f;

        // foreach (RewindableMovement rewindableMovement in dangers)
        // {
        //     float newTime = 9999f;
        //     if (rewindableMovement.GetType() == typeof(EnemyMovement))
        //     {
        //         if (!(rewindableMovement as EnemyMovement).WillKillRevenant(out newTime))
        //         {
        //             continue;
        //         }
        //     }
        //     else
        //     {
        //         if (!(rewindableMovement as BulletMovement).WillKillRevenant(out newTime))
        //         {
        //             continue;
        //         }
        //     }

        //     //Debug.Log("New Time: " + newTime + " from " + rewindableMovement.gameObject.name);
        //     // Debug.Log("Time: " + newTime + " from " + rewindableMovement.gameObject.name);
        //     if (newTime < lowestDeathTime)
        //     {
        //         lowestDeathTime = newTime;
        //     }
        // }

        // if (lowestDeathTime < 9999f)
        // {
        //     //Debug.Log("Closest object time: " + lowestDeathTime);
        //     pocketwatchUI.SetDeathTime(lowestDeathTime);
        // }
        // else
        // {
        //     pocketwatchUI.SetDeathTime(-1f);
        // }
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
