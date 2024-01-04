using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyStateMachine enemyStateMachine;

    [SerializeField]
    private List<WeakPoint> enemyWeakPoints = new List<WeakPoint>();

    private void Awake()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        foreach (WeakPoint weakPoint in enemyWeakPoints)
        {
            weakPoint.OnHit += Die;
        }
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in enemyWeakPoints)
        {
            weakPoint.OnHit -= Die;
        }
    }

    private void Die()
    {
        enemyStateMachine.EnterDeadState();
    }
}
