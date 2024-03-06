using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private const float SFX_VOLUME = 1f;

    public static void PlaySFX(AudioClip clip, float volume, Vector3 originPosition)
    {
        AudioSource.PlayClipAtPoint(clip, originPosition, volume * SFX_VOLUME);
    }
}
