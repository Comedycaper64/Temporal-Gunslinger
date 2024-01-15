using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayback : RewindableMovement
{
    // private float vfxPlayRate;
    // private float simulationTime = 0.0f;
    // private float startTime = 0.0f;
    // private bool effectPlaying;
    private VisualEffect visualEffect;

    private void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        //visualEffect.Simulate(startTime);
    }

    // private void Update()
    // {
    //     //Little jank
    //     if (vfxPlayRate != speed)
    //     {
    //         vfxPlayRate = speed;
    //         ChangeVFXPlayRate(vfxPlayRate);
    //     }

    //     if (!effectPlaying)
    //     {
    //         return;
    //     }

    //     // visualEffect.Stop();

    //     // bool useAutoRandomSeed = visualEffect.resetSeedOnPlay;
    //     // visualEffect.resetSeedOnPlay = false;

    //     // visualEffect.Play();

    //     // float deltaTime = visualEffect.
    //     //     ? Time.unscaledDeltaTime
    //     //     : Time.deltaTime;
    //     simulationTime += (Time.deltaTime * visualEffect.playRate) * vfxPlayRate;

    //     float currentSimulationTime = startTime + simulationTime;
    //     visualEffect.Simulate(currentSimulationTime);

    //     //visualEffect.resetSeedOnPlay = useAutoRandomSeed;

    //     if (currentSimulationTime < 0.0f)
    //     {
    //         visualEffect.Play();
    //         visualEffect.Stop();
    //     }
    // }

    // private void ChangeVFXPlayRate(float playRate)
    // {
    //     // visualEffect.playRate = playRate;
    //     // visualEffect.Simulate(1);
    // }

    public void PlayEffect()
    {
        visualEffect.Play();

        //ToggleMovement(true);
    }
}
