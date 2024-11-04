using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // [SerializeField]
    // private float MUSIC_VOLUME = 1f;
    // private const float SFX_VOLUME = 1f;
    private const float SLOW_PITCH = 0.4f;
    private const float MIN_PITCH_VARIATION = 0.9f;
    private const float MAX_PITCH_VARIATION = 1.1f;
    private const float FADE_SPEED = 0.1f;
    private const float TICK_SFX_INTERVAL = 1f;
    private bool tick;
    private bool fadeIn;
    private bool fadeOut;

    [SerializeField]
    private AudioSource musicAudioSource;

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
    }

    private void OnDisable()
    {
        OptionsManager.OnMasterVolumeUpdated -= UpdateMasterVolume;
        OptionsManager.OnMusicVolumeUpdated -= UpdateMusicVolume;
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
            musicAudioSource.volume =
                PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume();
        }

        StartCoroutine(ClockTickSFX());
    }

    private void Update()
    {
        if (fadeIn)
        {
            musicAudioSource.volume += FADE_SPEED * Time.unscaledDeltaTime;
            if (
                musicAudioSource.volume
                >= PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume()
            )
            {
                fadeIn = false;
                musicAudioSource.volume =
                    PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume();
            }
        }

        if (fadeOut)
        {
            musicAudioSource.volume -= FADE_SPEED * Time.unscaledDeltaTime;
            if (musicAudioSource.volume <= 0f)
            {
                fadeOut = false;
            }
        }
    }

    private static AudioSource PlaySFXClip(
        AudioClip clip,
        Vector3 position,
        float volume,
        float pitch
    )
    {
        var tempGameObject = new GameObject("CustomsOneShotAudio");
        tempGameObject.transform.position = position;
        var tempAudioSource = tempGameObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = volume;
        tempAudioSource.pitch = pitch;
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
        bool varyPitch = true
    )
    {
        if (!Application.isPlaying)
        {
            return null;
        }

        if (RewindManager.bRewinding)
        {
            return null;
        }

        float pitchVariance = 1f;

        if (varyPitch)
        {
            pitchVariance = Random.Range(MIN_PITCH_VARIATION, MAX_PITCH_VARIATION);
        }

        if (GameManager.bLevelActive && useSlowdownSettings)
        {
            return PlaySFXClip(
                clip,
                originPosition,
                volume * PlayerOptions.GetMasterVolume() * PlayerOptions.GetSFXVolume(),
                enumToPitch[(PitchEnum)pitchEnum] * SLOW_PITCH * pitchVariance
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
        musicAudioSource.volume = newVolume;
    }

    public void FadeOutMusic()
    {
        fadeOut = true;
        fadeIn = false;
    }

    public void FadeInMusic()
    {
        SetMusicAudioSourceVolume(0f);
        fadeIn = true;
        fadeOut = false;
    }

    public void SetMusicTrack(AudioClip newTrack)
    {
        musicAudioSource.clip = newTrack;
        musicAudioSource.Play();
    }

    private void UpdateMasterVolume(object sender, float newVolume)
    {
        SetMusicAudioSourceVolume(PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume());
    }

    private void UpdateMusicVolume(object sender, float newVolume)
    {
        SetMusicAudioSourceVolume(PlayerOptions.GetMasterVolume() * PlayerOptions.GetMusicVolume());
    }
}
