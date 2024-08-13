using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingChandelier : MonoBehaviour, IReactable
{
    [SerializeField]
    private BulletStateMachine chandelierBullet;

    [SerializeField]
    private EnvironmentDamager chandelierDamager;

    [SerializeField]
    private GameObject chandelierAirResistFX;

    [SerializeField]
    private WeakPoint chandelierRope;

    private void Awake()
    {
        chandelierRope.OnHit += StartFalling;
        chandelierDamager.OnCrush += KillBullet;
        chandelierDamager.gameObject.SetActive(false);
        chandelierAirResistFX.SetActive(false);
    }

    private void OnDisable()
    {
        chandelierRope.OnHit -= StartFalling;
        chandelierDamager.OnCrush -= KillBullet;
    }

    private void StartFalling(object sender, EventArgs e)
    {
        GameObject weakPoint = (sender as WeakPoint).gameObject;
        DestroyWeakPoint.WeakPointDestroyed(this, weakPoint);
        chandelierBullet.SwitchToActive();
        chandelierDamager.gameObject.SetActive(true);
        chandelierAirResistFX.SetActive(true);
    }

    private void KillBullet()
    {
        //put bullet into death state
        Debug.Log("Destroy chandelier");
    }

    public void UndoReaction()
    {
        chandelierDamager.gameObject.SetActive(false);
        chandelierAirResistFX.SetActive(false);
    }
}
