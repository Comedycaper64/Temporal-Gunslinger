using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const float MUSIC_VOLUME = 1f;
    private const float SFX_VOLUME = 1f;
    private const float SLOW_PITCH = 0.4f;
    private const float FADE_SPEED = 0.5f;
    private const float TICK_SFX_INTERVAL = 1f;
    private bool tick;
    private bool fadeIn;
    private bool fadeOut;
    private AudioSource musicAudioSource;

    [SerializeField]
    private bool fadeInAtStart;

    [SerializeField]
    private AudioClip clockTick1;

    [SerializeField]
    private AudioClip clockTick2;

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
        if (fadeInAtStart)
        {
            FadeInMusic();
        }

        StartCoroutine(ClockTickSFX());
    }

    private void Update()
    {
        if (fadeIn)
        {
            musicAudioSource.volume += FADE_SPEED * Time.deltaTime;
            if (musicAudioSource.volume >= MUSIC_VOLUME)
            {
                fadeIn = false;
            }
        }

        if (fadeOut)
        {
            musicAudioSource.volume -= FADE_SPEED * Time.deltaTime;
            if (musicAudioSource.volume <= 0f)
            {
                fadeOut = false;
            }
        }
    }

    private static void PlaySFXClip(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        var tempGameObject = new GameObject("CustomsOneShotAudio");
        tempGameObject.transform.position = position;
        var tempAudioSource = tempGameObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = volume;
        tempAudioSource.pitch = pitch;
        tempAudioSource.Play();
        Destroy(tempGameObject, clip.length);
    }

    public static void PlaySFX(AudioClip clip, float volume, Vector3 originPosition)
    {
        if (RewindManager.bRewinding)
        {
            return;
        }

        if (GameManager.bLevelActive)
        {
            PlaySFXClip(clip, originPosition, volume, SLOW_PITCH);
        }
        else
        {
            AudioSource.PlayClipAtPoint(clip, originPosition, volume * SFX_VOLUME);
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
                    0.3f * SFX_VOLUME
                );
            }
            else
            {
                AudioSource.PlayClipAtPoint(
                    clockTick2,
                    Camera.main.transform.position,
                    0.3f * SFX_VOLUME
                );
            }
        }
        StartCoroutine(ClockTickSFX());
    }

    public void FadeOutMusic()
    {
        fadeOut = true;
        fadeIn = false;
    }

    public void FadeInMusic()
    {
        fadeIn = true;
        fadeOut = false;
    }

    public void SetMusicTrack(AudioClip newTrack)
    {
        musicAudioSource.clip = newTrack;
        musicAudioSource.Play();
    }
}
