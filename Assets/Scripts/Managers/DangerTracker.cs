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
        BulletMovement.OnRedirect += SetNewDeathTime;
    }

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

            if (newTime < lowestDeathTime)
            {
                lowestDeathTime = newTime;
            }
        }

        if (lowestDeathTime > 0f)
        {
            pocketwatchUI.SetDeathTime(lowestDeathTime);
        }
    }
}
