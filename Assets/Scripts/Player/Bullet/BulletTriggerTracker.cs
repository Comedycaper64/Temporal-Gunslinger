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
        if (other.TryGetComponent<BulletPossessTarget>(out BulletPossessTarget possessTarget))
        {
            if (possessTarget != thisPossessable)
            {
                thisPossessable.AddPossessable(possessTarget);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BulletPossessTarget>(out BulletPossessTarget possessTarget))
        {
            thisPossessable.RemovePossessable(possessTarget);
        }
    }
}
