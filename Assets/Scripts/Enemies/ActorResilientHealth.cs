using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorResilientHealth : MonoBehaviour
{
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
            weakPoint.OnHit += Die;
        }
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit -= Die;
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
}
