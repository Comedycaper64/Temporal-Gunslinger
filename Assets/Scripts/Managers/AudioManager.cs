using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private const float SFX_VOLUME = 1f;

    public void PlaySFX(AudioClip clip, Vector3 originPosition)
    {
        AudioSource.PlayClipAtPoint(clip, originPosition, 1f);
    }
}
