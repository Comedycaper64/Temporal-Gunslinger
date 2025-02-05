using System;
using UnityEngine;

public class EnemyMovement : RewindableMovement
{
    [SerializeField]
    private bool moveToTarget = true;

    //private Vector3 revenantOffset = new Vector3(0f, 1.4f, -0.25f);

    [SerializeField]
    private Transform damagePoint;

    [SerializeField]
    private Sprite pocketwatchDangerSprite;

    private Transform movementTarget;

    public static Action OnEnemyMovementChange;

    private void Start()
    {
        movementTarget = GameManager.GetRevenant();
    }

    protected override void StartMovement()
    {
        base.StartMovement();

        if (!moveToTarget)
        {
            return;
        }

        float deathTime;
        WillKillRevenant(out deathTime);
        DangerTracker.dangers.Add(this, new PocketwatchDanger(pocketwatchDangerSprite, deathTime));
        OnEnemyMovementChange?.Invoke();

        transform.LookAt(movementTarget.position); //+ revenantOffset);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }

    protected override void StopMovement()
    {
        base.StopMovement();
        DangerTracker.dangers.Remove(this);
        OnEnemyMovementChange?.Invoke();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DangerTracker.dangers.Remove(this);
    }

    private void Update()
    {
        transform.position += transform.forward * GetSpeed() * Time.deltaTime;
    }

    private bool WillKillRevenant(out float deathTime)
    {
        if (Mathf.Abs(GetUnscaledSpeed()) > 0f)
        {
            // Debug.Log(
            //     "Damage Point Position: "
            //         + damagePoint.position
            //         + ", Target Position: "
            //         + movementTarget.position
            // );

            float distanceToTarget = Vector3.Distance(
                damagePoint.position,
                movementTarget.position //+ revenantOffset
            );
            float timeToTarget = distanceToTarget / Mathf.Abs(GetStartSpeed());
            deathTime = timeToTarget;
            return true;
        }
        else
        {
            deathTime = -1f;
            return false;
        }
    }
}

// using System;
// using UnityEngine;

// public class EnemyMovement : RewindableMovement
// {
//     [SerializeField]
//     private bool moveToTarget = true;

//     //private Vector3 revenantOffset = new Vector3(0f, 1.4f, -0.25f);

//     [SerializeField]
//     private Transform damagePoint;

//     [SerializeField]
//     private Sprite pocketwatchDangerSprite;

//     private Transform movementTarget;

//     public static EventHandler<PocketwatchDanger> OnEnemyMovementChange;

//     private void Start()
//     {
//         movementTarget = GameManager.GetRevenant();
//     }

//     protected override void StartMovement()
//     {
//         base.StartMovement();

//         if (!moveToTarget)
//         {
//             return;
//         }

//         float deathTime;
//         WillKillRevenant(out deathTime);

//         PocketwatchDanger danger = new PocketwatchDanger(pocketwatchDangerSprite, deathTime);

//         DangerTracker.Instance.AddDanger(this, danger);
//         OnEnemyMovementChange?.Invoke(this, danger);

//         transform.LookAt(movementTarget.position); //+ revenantOffset);
//         transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
//     }

//     protected override void StopMovement()
//     {
//         base.StopMovement();
//         DangerTracker.Instance.RemoveDanger(this);
//         //OnEnemyMovementChange?.Invoke();
//     }

//     protected override void OnDisable()
//     {
//         base.OnDisable();
//         DangerTracker.Instance.RemoveDanger(this);
//     }

//     private void Update()
//     {
//         transform.position += transform.forward * GetSpeed() * Time.deltaTime;
//     }

//     private bool WillKillRevenant(out float deathTime)
//     {
//         if (Mathf.Abs(GetUnscaledSpeed()) > 0f)
//         {
//             // Debug.Log(
//             //     "Damage Point Position: "
//             //         + damagePoint.position
//             //         + ", Target Position: "
//             //         + movementTarget.position
//             // );

//             float distanceToTarget = Vector3.Distance(
//                 damagePoint.position,
//                 movementTarget.position //+ revenantOffset
//             );
//             float timeToTarget = distanceToTarget / Mathf.Abs(GetStartSpeed());
//             deathTime = timeToTarget;
//             return true;
//         }
//         else
//         {
//             deathTime = -1f;
//             return false;
//         }
//     }
// }
