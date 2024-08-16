using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerTracker : MonoBehaviour
{
    private PocketwatchUI pocketwatchUI;
    public static List<RewindableMovement> dangers = new List<RewindableMovement>();

    private void Awake()
    {
        pocketwatchUI = GetComponent<PocketwatchUI>();
        EnemyDeadState.OnEnemyDeadChange += SetNewDeathTime;
        BulletMovement.OnChangeDirection += SetNewDeathTime;
        pocketwatchUI.OnShowUI += SetNewDeathTime;
    }

    //MAY NEED OPTIMISING: Locked on Bullets redirect every frame. Potentially make it so that it stores death times and only recalculates those that change?

    private void SetNewDeathTime()
    {
        float lowestDeathTime = 9999f;

        foreach (RewindableMovement rewindableMovement in dangers)
        {
            float newTime = 9999f;
            if (rewindableMovement.GetType() == typeof(EnemyMovement))
            {
                if (!(rewindableMovement as EnemyMovement).WillKillRevenant(out newTime))
                {
                    continue;
                }
            }
            else
            {
                if (!(rewindableMovement as BulletMovement).WillKillRevenant(out newTime))
                {
                    continue;
                }
            }

            //Debug.Log("New Time: " + newTime + " from " + rewindableMovement.gameObject.name);
            // Debug.Log("Time: " + newTime + " from " + rewindableMovement.gameObject.name);
            if (newTime < lowestDeathTime)
            {
                lowestDeathTime = newTime;
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
