using System;
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

        Collider weakPointCollider = weakPoint.GetComponent<Collider>();
        weakPointCollider.enabled = false;
        DissolveController weakPointDissolve = weakPoint.GetComponent<DissolveController>();
        weakPointDissolve.StartDissolve();
        FocusHighlight weakPointHighlight = weakPoint.GetComponent<FocusHighlight>();
        weakPointHighlight.ToggleHighlight(false);
        weakPointHighlight.enabled = false;

        StartReaction.ReactionStarted(this);

        chandelierBullet.SwitchToActive();
        chandelierDamager.gameObject.SetActive(true);
        chandelierAirResistFX.SetActive(true);
    }

    private void KillBullet()
    {
        chandelierBullet.SwitchToDeadState();
        //Debug.Log("Destroy chandelier");
    }

    public void UndoReaction()
    {
        chandelierRope.GetComponent<Collider>().enabled = true;
        chandelierRope.GetComponent<FocusHighlight>().enabled = true;
        chandelierRope.GetComponent<DissolveController>().StopDissolve();

        chandelierDamager.gameObject.SetActive(false);
        chandelierAirResistFX.SetActive(false);
    }
}
