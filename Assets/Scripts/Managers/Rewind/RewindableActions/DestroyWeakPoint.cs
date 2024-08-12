using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeakPoint : RewindableAction
{
    private MonoBehaviour sender;
    private Collider weakPointCollider;
    private DissolveController weakPointDissolve;
    private FocusHighlight weakPointHighlight;

    public static void WeakPointDestroyed(MonoBehaviour sender, GameObject weakPoint)
    {
        new DestroyWeakPoint(sender, weakPoint);
        //Debug.Log("Weak Point destroyed" + weakPoint.name);
    }

    private DestroyWeakPoint(MonoBehaviour sender, GameObject weakPoint)
    {
        this.sender = sender;
        weakPointCollider = weakPoint.GetComponent<Collider>();
        weakPointCollider.enabled = false;
        weakPointDissolve = weakPoint.GetComponent<DissolveController>();
        weakPointDissolve.StartDissolve();
        weakPointHighlight = weakPoint.GetComponent<FocusHighlight>();
        weakPointHighlight.ToggleHighlight(false);
        weakPointHighlight.enabled = false;

        Execute();
    }

    public override void Undo()
    {
        //Debug.Log("Weak Point undone" + weakPointCollider.name);
        if (sender.GetType() == typeof(BossHealth))
        {
            BossHealth bossHealth = sender as BossHealth;
            bossHealth.UndoDamage();
        }
        else if (sender.GetType() == typeof(FallingShelf))
        {
            FallingShelf fallingShelf = sender as FallingShelf;
            fallingShelf.UndoFall();
        }

        weakPointCollider.enabled = true;
        weakPointHighlight.enabled = true;
        weakPointDissolve.StopDissolve();
    }
}
