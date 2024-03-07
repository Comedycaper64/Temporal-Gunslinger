using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationVFXManager : MonoBehaviour
{
    [SerializeField]
    private List<VisualEffect> visualEffects = new List<VisualEffect>();

    // [SerializeField]
    // private List<VFXPlayback> rewindableVFX = new List<VFXPlayback>();

    public void PlayVFX(int vfxNum)
    {
        visualEffects[vfxNum].Reinit();
        visualEffects[vfxNum].Play();
    }

    //public void PlayRewindVFX(int vfxNum) { }
}
