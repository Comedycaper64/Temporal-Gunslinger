using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationVFXManager : MonoBehaviour
{
    [SerializeField]
    private List<VFXPlayback> visualEffects = new List<VFXPlayback>();

    // [SerializeField]
    // private List<VFXPlayback> rewindableVFX = new List<VFXPlayback>();

    public void PlayVFX(int vfxNum)
    {
        visualEffects[vfxNum].PlayEffect();
    }

    //public void PlayRewindVFX(int vfxNum) { }
}
