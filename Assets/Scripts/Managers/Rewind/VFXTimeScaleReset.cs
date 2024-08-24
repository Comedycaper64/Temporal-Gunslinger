using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTimeScaleReset : MonoBehaviour
{
    private VFXPlayback vFXPlayback;

    private void Awake()
    {
        vFXPlayback = GetComponent<VFXPlayback>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += StopEffectPlay;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= StopEffectPlay;
    }

    private void StopEffectPlay(object sender, StateEnum state)
    {
        if (state == StateEnum.inactive)
        {
            vFXPlayback.SetIsEffectPlaying(false);
        }
    }
}
