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
        TimedDanger.OnTimedDangerChange += SetDeathTimes;
        BulletMovement.OnChangeDirection += SetNewDeathTime;
        pocketwatchUI.OnShowUI += SetDeathTimes;
    }

    private void OnDisable()
    {
        EnemyMovement.OnEnemyMovementChange -= SetDeathTimes;
        TimedDanger.OnTimedDangerChange -= SetDeathTimes;
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

        SetDeathTimes();
    }

    private void SetDeathTimes()
    {
        //Get lowest death time
        //Set it in pocketweatch ui



        List<PocketwatchDanger> dangers = DangerTracker.dangers.Values.ToList();

        pocketwatchUI.ClearDeathTimes();

        foreach (PocketwatchDanger danger in dangers)
        {
            if (danger.deathTime < 0f)
            {
                continue;
            }

            pocketwatchUI.SetDeathTime(danger.deathTime, danger.deathIcon);
        }
    }
}
