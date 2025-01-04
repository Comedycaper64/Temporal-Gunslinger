using System;
using UnityEngine;

public class HuskPustule : MonoBehaviour
{
    [SerializeField]
    private WeakPoint pustuleWeakPoint;

    [SerializeField]
    private BulletStateMachine homingVirus;

    [SerializeField]
    private VFXPlayback pustuleBurstVFX;

    [SerializeField]
    private AudioClip virusSpawnSFX;

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
        AudioManager.PlaySFX(virusSpawnSFX, 1f, 2, transform.position, false);
        pustuleBurstVFX.PlayEffect();
    }
}
