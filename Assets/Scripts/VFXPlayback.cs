using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayback : RewindableMovement
{
    private float vfxPlayRate;
    private float simulationTime = 0.0f;
    private bool effectPlaying;
    private VisualEffect visualEffect;
    private const string TIME_VARIABLE = "Time";

    private void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (!effectPlaying)
        {
            return;
        }
        simulationTime += Time.deltaTime * speed;
        visualEffect.SetFloat(TIME_VARIABLE, simulationTime);
    }

    public void StopEffect()
    {
        visualEffect.Reinit();
        visualEffect.Stop();
        effectPlaying = false;
        simulationTime = 0.0f;
        ToggleMovement(false);
    }

    public void PlayEffect()
    {
        visualEffect.Play();
        simulationTime = 0.0f;
        effectPlaying = true;
        ToggleMovement(true);
        VFXPlayed.VFXPlay(this);
    }
}