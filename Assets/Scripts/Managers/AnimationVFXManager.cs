using System.Collections.Generic;
using UnityEngine;

public class AnimationVFXManager : MonoBehaviour
{
    [SerializeField]
    private List<VFXPlayback> visualEffects = new List<VFXPlayback>();

    public void PlayVFX(int vfxNum)
    {
        visualEffects[vfxNum].PlayEffect();
    }

    public void StopVFX(int vfxNum)
    {
        visualEffects[vfxNum].StopEffect();
    }
}
