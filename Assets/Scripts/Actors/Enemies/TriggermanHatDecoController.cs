using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggermanHatDecoController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] decorations;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += SetDecoMass;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= SetDecoMass;
    }

    private void SetDecoMass(object sender, StateEnum state)
    {
        float desiredMass = 25f;
        if (state == StateEnum.active)
        {
            desiredMass = 0.25f;
        }

        foreach (Rigidbody deco in decorations)
        {
            deco.mass = desiredMass;
        }
    }
}
