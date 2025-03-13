using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // [SerializeField]
    // private float MUSIC_VOLUME = 1f;
    // private const float SFX_VOLUME = 1f;
    //private const float SLOW_PITCH = 0.4f;
    private bool levelActive = false;
    private const float MIN_PITCH_VARIATION = 0.9f;
    private const float MAX_PITCH_VARIATION = 1.1f;
    private const float FADE_SPEED = 0.25f;
    private const float TICK_SFX_INTERVAL = 1f;
    private const float EMPHASISED_VOLUME = 1.25f;
    private const float REDUCED_VOLUME = 0.75f;
    private bool tick;
    private float fadeCounter = 0f;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool activeSwitch = false;
    private bool inactiveSwitch = false;

    [SerializeField]
    private AudioSource leadMusicAudioSource;

    [SerializeField]
    private AudioSource backingMusicAudioSource;

    [SerializeField]
    private AudioMixerGroup mixSetter;
    private static AudioMixerGroup slowdownMixer;

    [SerializeField]
    private bool fadeInAtStart;

    [SerializeField]
    private AudioClip clockTick1;

    [SerializeField]
    private AudioClip clockTick2;

    private enum PitchEnum
    {
        normal,
        twentyFive,
        fifty,
        seventyFive,
        onetwentyfive,
        onefifty,
        oneSeventyFive,
        twohundred
    }

    private static Dictionary<PitchEnum, float> enumToPitch = new Dictionary<PitchEnum, float>();

    private void OnEnable()
    {
        OptionsManager.OnMasterVolumeUpdated += UpdateMasterVolume;
        OptionsManager.OnMusicVolumeUpdated += UpdateMusicVolume;

        GameManager.OnGameStateChange += UpdateMusicTracks;

        slowdownMixer = mixSetter;
    }

    private void OnDisable()
    {
        OptionsManager.OnMasterVolumeUpdated -= UpdateMasterVolume;
        OptionsManager.OnMusicVolumeUpdated -= UpdateMusicVolume;

        GameManager.OnGameStateChange -= UpdateMusicTracks;
    }

    private void Start()
    {
        if (enumToPitch.Count == 0)
        {
            enumToPitch.Add(PitchEnum.normal, 1f);
            enumToPitch.Add(PitchEnum.twentyFive, 0.25f);
            enumToPitch.Add(PitchEnum.fifty, 0.5f);
            enumToPitch.Add(PitchEnum.seventyFive, 0.75f);
            enumToPitch.Add(PitchEnum.onetwentyfive, 1.25f);
            enumToPitch.Add(PitchEnum.onefifty, 1.5f);
            enumToPitch.Add(PitchEnum.oneSeventyFive, 1.75f);
            enumToPitch.Add(PitchEnum.twohundred, 2f);
        }

        //musicAudioSource = GetComponent<AudioSource>();
        if (fadeInAtStart)
        {
            FadeInMusic();
        }
        else
        {
            leadMusicAudioSource.volume =
                PlayerOptions.GetMasterVolume()
                * PlayerOptions.GetMusicVolume()
                * EMPHASISED_VOLUME;
            backingMusicAudioSource.volume =
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME;
        }

        StartCoroutine(ClockTickSFX());
    }

    private void Update()
    {
        if (fadeIn)
        {
            FadeIn();
        }
        else if (fadeOut)
        {
            FadeOut();
        }

        if (activeSwitch)
        {
            SwitchToActiveMusic();
        }
        else if (inactiveSwitch)
        {
            SwitchToInactiveMusic();
        }
    }

    private void SwitchToActiveMusic()
    {
        fadeCounter += FADE_SPEED * 2f * Time.unscaledDeltaTime;

        if (fadeCounter < 1f)
        {
            float leadVolume = Mathf.Lerp(
                PlayerOptions.GetMasterVolume()
                    * PlayerOptions.GetMusicVolume()
                    * EMPHASISED_VOLUME,
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME,
                fadeCounter
            );
            leadMusicAudioSource.volume = leadVolume;

            float backingVolume = Mathf.Lerp(
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME,
                PlayerOptions.GetMasterVolume()
                    * PlayerOptions.GetMusicVolume()
                    * EMPHASISED_VOLUME,
                fadeCounter
            );
            backingMusicAudioSource.volume = backingVolume;
        }
        else
        {
            activeSwitch = false;
        }
    }

    private void SwitchToInactiveMusic()
    {
        fadeCounter += FADE_SPEED * 2f * Time.unscaledDeltaTime;

        if (fadeCounter < 1f)
        {
            float leadVolume = Mathf.Lerp(
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME,
                PlayerOptions.GetMasterVolume()
                    * PlayerOptions.GetMusicVolume()
                    * EMPHASISED_VOLUME,
                fadeCounter
            );
            leadMusicAudioSource.volume = leadVolume;

            float backingVolume = Mathf.Lerp(
                PlayerOptions.GetMasterVolume()
                    * PlayerOptions.GetMusicVolume()
                    * EMPHASISED_VOLUME,
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME,
                fadeCounter
            );
            backingMusicAudioSource.volume = backingVolume;
        }
        else
        {
            inactiveSwitch = false;
        }
    }

    private void FadeIn()
    {
        fadeCounter += FADE_SPEED * Time.unscaledDeltaTime;

        if (fadeCounter < 1f)
        {
            float leadVolume = Mathf.Lerp(
                0f,
                PlayerOptions.GetMasterVolume()
                    * PlayerOptions.GetMusicVolume()
                    * EMPHASISED_VOLUME,
                fadeCounter
            );
            leadMusicAudioSource.volume = leadVolume;

            float backingVolume = Mathf.Lerp(
                0f,
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME,
                fadeCounter
            );
            backingMusicAudioSource.volume = backingVolume;
        }
        else
        {
            leadMusicAudioSource.volume =
                PlayerOptions.GetMasterVolume()
                * PlayerOptions.GetMusicVolume()
                * EMPHASISED_VOLUME;

            backingMusicAudioSource.volume =
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME;
            fadeIn = false;
        }
    }

    private void FadeOut()
    {
        fadeCounter += FADE_SPEED * 1.5f * Time.unscaledDeltaTime;

        if (fadeCounter < 1f)
        {
            float leadVolume = Mathf.Lerp(
                PlayerOptions.GetMasterVolume()
                    * PlayerOptions.GetMusicVolume()
                    * EMPHASISED_VOLUME,
                0f,
                fadeCounter
            );
            leadMusicAudioSource.volume = leadVolume;

            float backingVolume = Mathf.Lerp(
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume() * REDUCED_VOLUME,
                0f,
                fadeCounter
            );
            backingMusicAudioSource.volume = backingVolume;
        }
        else
        {
            fadeOut = false;
        }
    }

    private static AudioSource PlaySFXClip(
        AudioClip clip,
        Vector3 position,
        float volume,
        float pitch,
        bool useSlowdownMixer = false
    )
    {
        var tempGameObject = new GameObject("CustomsOneShotAudio");
        tempGameObject.transform.position = position;
        var tempAudioSource = tempGameObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = volume;
        tempAudioSource.pitch = pitch;

        if (useSlowdownMixer)
        {
            tempAudioSource.outputAudioMixerGroup = slowdownMixer;
        }

        tempAudioSource.Play();
        Destroy(tempGameObject, clip.length);

        return tempAudioSource;
    }

    public static AudioSource PlaySFX(
        AudioClip clip,
        float volume,
        int pitchEnum,
        Vector3 originPosition,
        bool useSlowdownSettings = true,
        bool varyPitch = true,
        bool playDuringRewind = false
    )
    {
        if (!Application.isPlaying)
        {
            return null;
        }

        if (RewindManager.bRewinding && !playDuringRewind)
        {
            return null;
        }

        float pitchVariance = 1f;

        if (varyPitch)
        {
            pitchVariance = Random.Range(MIN_PITCH_VARIATION, MAX_PITCH_VARIATION);
        }

        if (RewindManager.bTimerActive && useSlowdownSettings)
        {
            return PlaySFXClip(
                clip,
                originPosition,
                volume * PlayerOptions.GetMasterVolume() * PlayerOptions.GetSFXVolume(),
                enumToPitch[(PitchEnum)pitchEnum] * pitchVariance,
                true
            );
        }
        else
        {
            return PlaySFXClip(
                clip,
                originPosition,
                volume * PlayerOptions.GetMasterVolume() * PlayerOptions.GetSFXVolume(),
                enumToPitch[(PitchEnum)pitchEnum] * pitchVariance
            );
        }
    }

    private IEnumerator ClockTickSFX()
    {
        yield return new WaitForSeconds(TICK_SFX_INTERVAL);
        tick = !tick;
        if (RewindManager.bRewinding)
        {
            if (!tick)
            {
                AudioSource.PlayClipAtPoint(
                    clockTick1,
                    Camera.main.transform.position,
                    1f * PlayerOptions.GetMasterVolume() * PlayerOptions.GetSFXVolume()
                );
            }
            else
            {
                AudioSource.PlayClipAtPoint(
                    clockTick2,
                    Camera.main.transform.position,
                    1f * PlayerOptions.GetMasterVolume() * PlayerOptions.GetSFXVolume()
                );
            }
        }
        StartCoroutine(ClockTickSFX());
    }

    private void SetMusicAudioSourceVolume(float newVolume)
    {
        fadeIn = false;
        fadeOut = false;
        inactiveSwitch = false;
        activeSwitch = false;

        if (levelActive)
        {
            leadMusicAudioSource.volume = newVolume * REDUCED_VOLUME;
            backingMusicAudioSource.volume = newVolume * EMPHASISED_VOLUME;
        }
        else
        {
            leadMusicAudioSource.volume = newVolume * EMPHASISED_VOLUME;
            backingMusicAudioSource.volume = newVolume * REDUCED_VOLUME;
        }
    }

    public void FadeOutMusic()
    {
        fadeOut = true;
        fadeIn = false;
        fadeCounter = 0f;
    }

    public void FadeInMusic()
    {
        SetMusicAudioSourceVolume(0f);
        fadeIn = true;
        fadeOut = false;
        fadeCounter = 0f;
    }

    public void SetMusicTrack(AudioClip newLeadTrack, AudioClip newBackingTrack)
    {
        leadMusicAudioSource.clip = newLeadTrack;
        leadMusicAudioSource.Play();

        backingMusicAudioSource.clip = newBackingTrack;

        if (backingMusicAudioSource.clip)
        {
            backingMusicAudioSource.Play();
        }
    }

    private void UpdateMasterVolume(object sender, float newVolume)
    {
        SetMusicAudioSourceVolume(PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume());
    }

    private void UpdateMusicVolume(object sender, float newVolume)
    {
        SetMusicAudioSourceVolume(PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume());
    }

    private void UpdateMusicTracks(object sender, StateEnum state)
    {
        if (state == StateEnum.active)
        {
            if (levelActive != true)
            {
                activeSwitch = true;
                inactiveSwitch = false;
                levelActive = true;
                fadeCounter = 0f;
            }
        }
        else
        {
            if (levelActive != false)
            {
                inactiveSwitch = true;
                activeSwitch = false;
                levelActive = false;
                fadeCounter = 0f;
            }
        }
    }
}
