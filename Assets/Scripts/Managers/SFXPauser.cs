using System;
using UnityEngine;

public class SFXPauser : MonoBehaviour
{
    [SerializeField]
    private bool pauseDuringLevel = true;

    [SerializeField]
    private bool loopingSFX = false;
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

        if (loopingSFX)
        {
            OptionsManager.OnMasterVolumeUpdated += UpdateVolume;
            OptionsManager.OnSFXVolumeUpdated += UpdateVolume;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleAudioSource;

        if (loopingSFX)
        {
            OptionsManager.OnMasterVolumeUpdated -= UpdateVolume;
            OptionsManager.OnSFXVolumeUpdated -= UpdateVolume;
        }
    }

    private void ToggleAudioSource(object sender, StateEnum newState)
    {
        if ((newState == StateEnum.active) && pauseDuringLevel)
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

    private void UpdateVolume(object sender, float e)
    {
        sfxAudioSource.volume = PlayerOptions.GetSFXVolume() * sfxVolume;
    }
}
