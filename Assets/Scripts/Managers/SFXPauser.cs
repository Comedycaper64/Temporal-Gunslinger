using UnityEngine;

public class SFXPauser : MonoBehaviour
{
    private AudioSource sfxAudioSource;

    [SerializeField]
    [Range(0f, 1f)]
    private float sfxVolume = 1f;

    private void Awake()
    {
        sfxAudioSource = GetComponent<AudioSource>();
        PlaySFX();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += ToggleAudioSource;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleAudioSource;
    }

    private void ToggleAudioSource(object sender, StateEnum newState)
    {
        if (newState == StateEnum.active)
        {
            PauseSFX();
        }
        else
        {
            PlaySFX();
        }
    }

    private void PauseSFX()
    {
        sfxAudioSource.volume = 0f;
    }

    private void PlaySFX()
    {
        // if (sfxAudioSource.isPlaying)
        // {
        //     return;
        // }

        sfxAudioSource.volume = PlayerOptions.GetSFXVolume() * sfxVolume;
    }
}
