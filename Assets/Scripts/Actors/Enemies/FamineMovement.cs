using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FamineMovement : RewindableMovement
{
    private SplineAnimate splineAnimate;

    [SerializeField]
    private VFXPlayback locustVFX;

    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    private void Update()
    {
        if (movementActive)
        {
            splineAnimate.elapsedTime += GetSpeed() * Time.deltaTime;
        }
    }

    public override void ToggleMovement(bool toggle)
    {
        base.ToggleMovement(toggle);

        if (toggle)
        {
            locustVFX.SetSpeed(10f);
        }
        else
        {
            locustVFX.SetSpeed(1f);
        }
    }

    public void ResetMovement()
    {
        splineAnimate.Restart(false);
    }
}
