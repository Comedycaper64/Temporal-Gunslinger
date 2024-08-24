using UnityEngine;

[CreateAssetMenu(fileName = "PlaySFX", menuName = "Cinematic Node/PlaySFXSO", order = 7)]
public class PlaySFXSO : CinematicNode
{
    public AudioClip soundEffect;

    [Range(0, 1)]
    public float sfxVolume;

    [Range(0, 7)]
    public int sfxPitch;
}
