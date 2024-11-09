using System;
using System.Collections.Generic;
using UnityEngine;

public class PustuleWall : MonoBehaviour, IReactable
{
    private int weakPointTracker;

    [SerializeField]
    private Collider wallCollider;

    [SerializeField]
    private DissolveController dissolveController;

    [SerializeField]
    private List<WeakPoint> weakPoints = new List<WeakPoint>();

    private void OnEnable()
    {
        weakPointTracker = 0;

        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPointTracker++;
            weakPoint.OnHit += Damage;
        }
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit -= Damage;
        }
    }

    private void Damage(object sender, EventArgs e)
    {
        weakPointTracker--;

        GameObject weakPoint = (sender as WeakPoint).gameObject;

        DestroyWeakPoint.WeakPointDestroyed(this, weakPoint);

        if (weakPointTracker <= 0)
        {
            wallCollider.enabled = false;
            dissolveController.StartDissolve();
        }
    }

    public void UndoReaction()
    {
        weakPointTracker++;

        if (wallCollider.enabled == false)
        {
            wallCollider.enabled = true;
            dissolveController.StopDissolve();
        }
    }
}
