using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSFXManager : MonoBehaviour
{
    [SerializeField]
    private float audioOutputHeight;

    public void PlaySFX(AnimationEvent animationEvent)
    {
        AudioClip sfxClip = animationEvent.objectReferenceParameter as AudioClip;
        Vector3 outputPosition = transform.position + new Vector3(0f, audioOutputHeight, 0f);
        AudioManager.PlaySFX(sfxClip, animationEvent.floatParameter, outputPosition);
    }
}
