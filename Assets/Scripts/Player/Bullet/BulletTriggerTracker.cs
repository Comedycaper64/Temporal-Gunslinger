using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTriggerTracker : MonoBehaviour
{
    [SerializeField]
    private BulletPossessTarget thisPossessable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHighlightable>(out IHighlightable target))
        {
            if (target != (IHighlightable)thisPossessable)
            {
                thisPossessable.AddHighlightable(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IHighlightable>(out IHighlightable target))
        {
            thisPossessable.RemoveHighlightable(target);
        }
    }
}
