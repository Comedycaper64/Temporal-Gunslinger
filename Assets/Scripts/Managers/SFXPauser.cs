using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPauser : MonoBehaviour
{
    private AudioSource sfxAudioSource;

    private void Start()
    {
        sfxAudioSource = GetComponent<AudioSource>();
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
        sfxAudioSource.Pause();
    }

    private void PlaySFX()
    {
        if (sfxAudioSource.isPlaying)
        {
            return;
        }

        sfxAudioSource.Play();
    }
}
