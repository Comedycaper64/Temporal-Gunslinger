using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHealth : MonoBehaviour
{
    private StateMachine stateMachine;

    [SerializeField]
    private List<WeakPoint> weakPoints = new List<WeakPoint>();

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

    private void Die()
    {
        stateMachine.SwitchToDeadState();
    }
}
