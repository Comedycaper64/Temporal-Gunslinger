using System;
using UnityEngine;

public class SFXPauser : MonoBehaviour
{
    [SerializeField]
    private bool pauseDuringLevel = true;

    [SerializeField]
    protected bool loopingSFX = false;
    protected AudioSource sfxAudioSource;

    [SerializeField]
    [Range(0f, 1f)]
    protected float sfxVolume = 1f;

    private void Awake()
    {
        sfxAudioSource = GetComponent<AudioSource>();
        PlaySFX();
    }

    protected virtual void OnEnable()
    {
        GameManager.OnGameStateChange += ToggleAudioSource;

        if (loopingSFX)
        {
            OptionsManager.OnMasterVolumeUpdated += UpdateVolume;
            OptionsManager.OnSFXVolumeUpdated += UpdateVolume;
        }
    }

    protected virtual void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleAudioSource;

        if (loopingSFX)
        {
            OptionsManager.OnMasterVolumeUpdated -= UpdateVolume;
            OptionsManager.OnSFXVolumeUpdated -= UpdateVolume;
        }
    }

    protected void ToggleAudioSource(object sender, StateEnum newState)
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

    protected virtual void PlaySFX()
    {
        // if (sfxAudioSource.isPlaying)
        // {
        //     return;
        // }

        sfxAudioSource.volume = PlayerOptions.GetSFXVolume() * sfxVolume;
    }

    protected virtual void UpdateVolume(object sender, float e)
    {
        sfxAudioSource.volume = PlayerOptions.GetSFXVolume() * sfxVolume;
    }
}
