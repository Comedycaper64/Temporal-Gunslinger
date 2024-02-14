using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPlayed : RewindableAction
{
    private VFXPlayback vFXPlayback;

    public static void VFXPlay(VFXPlayback vFXPlayback)
    {
        VFXPlayed vFXPlayed = new VFXPlayed(vFXPlayback);
    }

    public VFXPlayed(VFXPlayback vFXPlayback)
    {
        this.vFXPlayback = vFXPlayback;
        Execute();
    }

    public override void Undo()
    {
        vFXPlayback.StopEffect();
    }
}
