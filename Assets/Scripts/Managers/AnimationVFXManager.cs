using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationVFXManager : MonoBehaviour
{
    [SerializeField]
    private List<VisualEffect> visualEffects = new List<VisualEffect>();

    public void PlayVFX(int vfxNum)
    {
        visualEffects[vfxNum].Play();
    }
}
