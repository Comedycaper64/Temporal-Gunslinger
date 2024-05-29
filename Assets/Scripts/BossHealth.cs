using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private int lesserWeakPoints;
    private StateMachine stateMachine;

    [SerializeField]
    private List<WeakPoint> weakPoints = new List<WeakPoint>();

    [SerializeField]
    private WeakPoint finalWeakPoint;

    [SerializeField]
    private AudioClip deathSFX;

    private void OnEnable()
    {
        stateMachine = GetComponent<StateMachine>();
        lesserWeakPoints = 0;
        foreach (WeakPoint weakPoint in weakPoints)
        {
            lesserWeakPoints++;
            weakPoint.OnHit += Damage;
        }

        finalWeakPoint.OnHit += Die;
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit -= Damage;
        }

        finalWeakPoint.OnHit -= Die;
    }

    private void Die(object sender, EventArgs e)
    {
        stateMachine.SwitchToDeadState();

        if (deathSFX)
        {
            AudioManager.PlaySFX(deathSFX, 0.5f, 0, transform.position);
        }
    }

    private void Damage(object sender, EventArgs e)
    {
        lesserWeakPoints--;

        GameObject weakPoint = (sender as WeakPoint).gameObject;
        DestroyWeakPoint.WeakPointDestroyed(this, weakPoint);

        if (lesserWeakPoints <= 0)
        {
            finalWeakPoint.gameObject.SetActive(true);
        }
    }

    public void UndoDamage()
    {
        lesserWeakPoints++;

        if (finalWeakPoint.isActiveAndEnabled)
        {
            finalWeakPoint.gameObject.SetActive(false);
        }
    }

    //undo damage, adding lesser weakpoint, undoing final weak point spawn, stopping dissolve
}
