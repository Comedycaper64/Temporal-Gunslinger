using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorResilientHealth : MonoBehaviour
{
    private bool damaged = false;
    private StateMachine stateMachine;

    [SerializeField]
    private List<WeakPoint> weakPoints = new List<WeakPoint>();

    [SerializeField]
    private AudioClip deathSFX;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit += DamageTaken;
            weakPoint.OnCrush += Die;
        }
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit -= DamageTaken;
            weakPoint.OnCrush -= Die;
        }
    }

    private void DamageTaken(object sender, EventArgs e)
    {
        if (!damaged)
        {
            damaged = true;
            DamageEnemy.EnemyDamaged(this, stateMachine.GetDissolveController());
        }
        else
        {
            Die(this, null);
        }
    }

    private void Die(object sender, EventArgs e)
    {
        stateMachine.SwitchToDeadState();

        if (deathSFX)
        {
            AudioManager.PlaySFX(deathSFX, 0.5f, 0, transform.position);
        }
    }

    public void UndoDamage()
    {
        damaged = false;
    }
}
