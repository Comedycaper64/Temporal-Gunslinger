using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeakPoint : RewindableAction
{
    private BossHealth bossHealth;
    private Collider weakPointCollider;
    private DissolveController weakPointDissolve;
    private FocusHighlight weakPointHighlight;

    public static void WeakPointDestroyed(BossHealth bossHealth, GameObject weakPoint)
    {
        DestroyWeakPoint destroyWeakPoint = new DestroyWeakPoint(bossHealth, weakPoint);
        //Debug.Log("Weak Point destroyed" + weakPoint.name);
    }

    public DestroyWeakPoint(BossHealth bossHealth, GameObject weakPoint)
    {
        this.bossHealth = bossHealth;
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
        bossHealth.UndoDamage();
        weakPointCollider.enabled = true;
        weakPointHighlight.enabled = true;
        weakPointDissolve.StopDissolve();
    }
}
