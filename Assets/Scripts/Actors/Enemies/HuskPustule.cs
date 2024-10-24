using System;
using UnityEngine;

public class HuskPustule : MonoBehaviour
{
    private WeakPoint pustuleWeakPoint;

    [SerializeField]
    private BulletStateMachine homingVirus;

    [SerializeField]
    private VFXPlayback pustuleBurstVFX;

    private void Awake()
    {
        pustuleWeakPoint = GetComponent<WeakPoint>();
    }

    private void OnEnable()
    {
        pustuleWeakPoint.OnHit += BurstPustule;
    }

    private void OnDisable()
    {
        pustuleWeakPoint.OnHit -= BurstPustule;
    }

    private void BurstPustule(object sender, EventArgs e)
    {
        homingVirus.SwitchToActive();

        pustuleBurstVFX.PlayEffect();
    }
}
