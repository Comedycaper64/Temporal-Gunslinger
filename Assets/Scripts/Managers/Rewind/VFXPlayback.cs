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
    private const string PLAYBACK_VARIABLE = "Playback";

    [SerializeField]
    private bool controlTimeVariable = false;

    [SerializeField]
    private bool controlPlaybackVariable = false;

    [SerializeField]
    private bool bPlayOnStart;

    private void Start()
    {
        visualEffect = GetComponent<VisualEffect>();

        if (bPlayOnStart)
        {
            PlayEffect();
        }
    }

    private void Update()
    {
        if (!effectPlaying)
        {
            visualEffect.playRate = 1f;
            return;
        }

        if (controlPlaybackVariable)
        {
            visualEffect.SetFloat(PLAYBACK_VARIABLE, GetSpeed());
        }

        if (controlTimeVariable)
        {
            simulationTime += Time.deltaTime * GetSpeed();
            visualEffect.SetFloat(TIME_VARIABLE, simulationTime);
        }
        else
        {
            visualEffect.playRate = GetSpeed();
        }
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
        ToggleMovement(true);
        effectPlaying = true;

        if (controlTimeVariable)
        {
            simulationTime = 0.0f;
            VFXPlayed.VFXPlay(this);
        }
    }
}
