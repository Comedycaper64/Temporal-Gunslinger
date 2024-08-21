using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCarrier : RewindableMovement
{
    [SerializeField]
    private Renderer flameShaderRenderer;

    [SerializeField]
    private FlameCarrier adjacentCarrier;

    //subscribe to a falling object and start flame when it finishes

    // in update increase flame shader. when fully aflame start flame in adjacent carrier

    //use a rewindable action to stop flame

    public void StartFlame() { }
}
