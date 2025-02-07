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

        //Debug.Log((sender as BulletMovement).gameObject.name + " " + danger.deathTime);

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

// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// public class PocketwatchDanger
// {
//     public PocketwatchDanger(Sprite deathIcon, float deathTime = -1, bool dirty = true)
//     {
//         this.deathTime = deathTime;
//         this.deathIcon = deathIcon;
//         this.dirty = dirty;
//     }

//     public float deathTime;
//     public Sprite deathIcon;
//     public bool dirty;
// }

// public class DangerTracker : MonoBehaviour
// {
//     public static DangerTracker Instance { get; private set; }
//     private PocketwatchUI pocketwatchUI;
//     public Dictionary<RewindableMovement, PocketwatchDanger> dangers =
//         new Dictionary<RewindableMovement, PocketwatchDanger>();

//     //private static List<bool> dangerUpdateTracker = new List<bool>();

//     private void Awake()
//     {
//         if (Instance != null)
//         {
//             Debug.LogError("There's more than one Danger Tracker! " + transform + " - " + Instance);
//             Destroy(gameObject);
//             return;
//         }
//         Instance = this;

//         pocketwatchUI = GetComponent<PocketwatchUI>();
//     }

//     private void OnEnable()
//     {
//         EnemyMovement.OnEnemyMovementChange += SetNewDeathTime;
//         BulletMovement.OnChangeDirection += SetNewDeathTime;
//         pocketwatchUI.OnShowUI += SetDeathTimes;
//     }

//     private void OnDisable()
//     {
//         EnemyMovement.OnEnemyMovementChange -= SetNewDeathTime;
//         BulletMovement.OnChangeDirection -= SetNewDeathTime;
//         pocketwatchUI.OnShowUI -= SetDeathTimes;
//     }

//     public void AddDanger(RewindableMovement movement, PocketwatchDanger danger)
//     {
//         Debug.Log("Current list: " + dangers.Count);
//         dangers.Add(movement, danger);
//     }

//     public void RemoveDanger(RewindableMovement movement)
//     {
//         if (dangers.TryGetValue(movement, out PocketwatchDanger danger))
//         {
//             pocketwatchUI.ClearDeathTime(danger);
//             dangers.Remove(movement);
//         }
//     }

//     private void SetNewDeathTime(object sender, PocketwatchDanger output)
//     {
//         RewindableMovement movement = sender as RewindableMovement;

//         if (!dangers.TryGetValue(movement, out PocketwatchDanger output))
//         {
//             return;
//         }

//         dangers[movement] = new PocketwatchDanger(
//             danger.deathIcon,
//             pocketwatchUI.GetCurrentPocketwatchTime() + danger.deathTime
//         );

//         //Debug.Log((sender as BulletMovement).gameObject.name + " " + newDeathTime);

//         SetDeathTimes();
//     }

//     private void SetDeathTimes()
//     {
//         //Get lowest death time
//         //Set it in pocketweatch ui

//         //float lowestDeathTime = 9999f;

//         List<PocketwatchDanger> pocketwatchDangers = dangers.Values.ToList();

//         //pocketwatchUI.ClearDeathTimes();

//         foreach (PocketwatchDanger danger in pocketwatchDangers)
//         {
//             if (danger.deathTime < 0f)
//             {
//                 pocketwatchUI.ClearDeathTime(danger);
//                 continue;
//             }

//             if (danger.dirty == false)
//             {
//                 continue;
//             }
//             else
//             {
//                 pocketwatchUI.ClearDeathTime(danger);
//             }

//             // if (deathTime < lowestDeathTime)
//             // {
//             //     lowestDeathTime = deathTime;
//             // }

//             pocketwatchUI.SetDeathTime(danger);
//         }

//         // if (lowestDeathTime < 9999f)
//         // {
//         //     //Debug.Log("Closest object time: " + lowestDeathTime);

//         // }
//         // else
//         // {
//         //     pocketwatchUI.SetDeathTime(-1f);
//         // }
//     }
// }
