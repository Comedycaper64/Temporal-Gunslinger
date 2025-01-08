using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PocketwatchDanger
{
    public PocketwatchDanger(Sprite deathIcon, float deathTime = -1)
    {
        this.deathTime = deathTime;
        this.deathIcon = deathIcon;
    }

    public float deathTime;
    public Sprite deathIcon;
}

public class DangerTracker : MonoBehaviour
{
    private PocketwatchUI pocketwatchUI;
    public static Dictionary<RewindableMovement, PocketwatchDanger> dangers =
        new Dictionary<RewindableMovement, PocketwatchDanger>();

    private void Awake()
    {
        pocketwatchUI = GetComponent<PocketwatchUI>();
    }

    private void OnEnable()
    {
        EnemyMovement.OnEnemyMovementChange += SetDeathTimes;
        BulletMovement.OnChangeDirection += SetNewDeathTime;
        pocketwatchUI.OnShowUI += SetDeathTimes;
    }

    private void OnDisable()
    {
        EnemyMovement.OnEnemyMovementChange -= SetDeathTimes;
        BulletMovement.OnChangeDirection -= SetNewDeathTime;
        pocketwatchUI.OnShowUI -= SetDeathTimes;
    }

    private void SetNewDeathTime(object sender, PocketwatchDanger danger)
    {
        RewindableMovement movement = sender as RewindableMovement;

        dangers[movement] = new PocketwatchDanger(
            danger.deathIcon,
            pocketwatchUI.GetCurrentPocketwatchTime() + danger.deathTime
        );

        //Debug.Log((sender as BulletMovement).gameObject.name + " " + newDeathTime);

        SetDeathTimes();
    }

    private void SetDeathTimes()
    {
        //Get lowest death time
        //Set it in pocketweatch ui

        //float lowestDeathTime = 9999f;

        List<PocketwatchDanger> dangers = DangerTracker.dangers.Values.ToList();

        pocketwatchUI.ClearDeathTimes();

        foreach (PocketwatchDanger danger in dangers)
        {
            if (danger.deathTime < 0f)
            {
                continue;
            }

            // if (deathTime < lowestDeathTime)
            // {
            //     lowestDeathTime = deathTime;
            // }

            pocketwatchUI.SetDeathTime(danger.deathTime, danger.deathIcon);
        }

        // if (lowestDeathTime < 9999f)
        // {
        //     //Debug.Log("Closest object time: " + lowestDeathTime);

        // }
        // else
        // {
        //     pocketwatchUI.SetDeathTime(-1f);
        // }
    }
}
