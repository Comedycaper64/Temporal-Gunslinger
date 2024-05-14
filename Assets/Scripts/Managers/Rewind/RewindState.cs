using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState : RewindableMovement
{
    public float GetTimeSpeed() => GetUnscaledSpeed();

    public float GetScaledSpeed() => GetSpeed();
}
