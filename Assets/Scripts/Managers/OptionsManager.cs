using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    private bool vSyncState;
    private bool focusSettingState;

    private const float SLIDER_MOD = 8f;
    private AudioSource sampleAudioSource;

    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private AudioClip sampleSFX;

    [SerializeField]
    private Slider voiceSlider;

    [SerializeField]
    private AudioClip sampleVoice;

    [SerializeField]
    private Slider gunSensitivitySlider;

    // [SerializeField]
    // private Slider gunSensitivityYSlider;

    [SerializeField]
    private Slider bulletSensitivitySlider;

    // [SerializeField]
    // private Slider bulletSensitivityYSlider;
    [SerializeField]
    private TextMeshProUGUI focusHoldButton;

    [SerializeField]
    private TextMeshProUGUI focusToggleButton;

    [SerializeField]
    private Image vsyncButtonImage;

    public static EventHandler<float> OnMasterVolumeUpdated;
    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<float> OnSFXVolumeUpdated;
    public static EventHandler<float> OnVoiceVolumeUpdated;
    public static EventHandler<float> OnGunSensitivityUpdated;
    public static EventHandler<float> OnBulletSensitivityUpdated;
    public static EventHandler<bool> OnVSyncUpdated;
    public static EventHandler<bool> OnFocusUpdated;

    private void OnEnable()
    {
        masterSlider.value = PlayerOptions.GetMasterVolume() * SLIDER_MOD;
        musicSlider.value = PlayerOptions.GetMusicVolume() * SLIDER_MOD;
        sfxSlider.value = PlayerOptions.GetSFXVolume() * SLIDER_MOD;
        voiceSlider.value = PlayerOptions.GetVoiceVolume() * SLIDER_MOD;

        gunSensitivitySlider.value = PlayerOptions.GetGunSensitivity() * SLIDER_MOD;
        //gunSensitivityYSlider.value = PlayerOptions.GetGunYSensitivity();
        bulletSensitivitySlider.value = PlayerOptions.GetBulletSensitivity() * SLIDER_MOD;
        //bulletSensitivityYSlider.value = PlayerOptions.GetBulletYSensitivity();

        vSyncState = PlayerOptions.GetVSync();

        if (vSyncState)
        {
            vsyncButtonImage.color = Color.white;
        }
        else
        {
            vsyncButtonImage.color = Color.gray;
        }

        focusSettingState = PlayerOptions.GetFocusSetting();
        UpdateFocusControlUI();

        sampleAudioSource = GetComponent<AudioSource>();
    }

    public void SetMasterVolume(float newVolume)
    {
        newVolume = newVolume / SLIDER_MOD;
        PlayerOptions.SetMasterVolume(newVolume);
        OnMasterVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetMusicVolume(float newVolume)
    {
        newVolume = newVolume / SLIDER_MOD;
        PlayerOptions.SetMusicVolume(newVolume);
        OnMusicVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        newVolume = newVolume / SLIDER_MOD;
        PlayerOptions.SetSFXVolume(newVolume);

        OnSFXVolumeUpdated?.Invoke(this, newVolume);

        if (!sampleAudioSource)
        {
            return;
        }

        sampleAudioSource.clip = sampleSFX;
        sampleAudioSource.volume = PlayerOptions.GetSFXVolume();
        sampleAudioSource.Play();
    }

    public void SetVoiceVolume(float newVolume)
    {
        newVolume = newVolume / SLIDER_MOD;
        PlayerOptions.SetVoiceVolume(newVolume);

        OnVoiceVolumeUpdated?.Invoke(this, newVolume);

        if (!sampleAudioSource)
        {
            return;
        }

        sampleAudioSource.clip = sampleVoice;
        sampleAudioSource.volume = PlayerOptions.GetVoiceVolume();
        sampleAudioSource.Play();
    }

    public void SetGunSensitivity(float newSensitivity)
    {
        newSensitivity = newSensitivity / SLIDER_MOD;
        PlayerOptions.SetGunXSensitivity(newSensitivity);
        OnGunSensitivityUpdated?.Invoke(
            this,
            newSensitivity
        //new Vector2(newSensitivity, PlayerOptions.GetGunYSensitivity())
        );
    }

    // public void SetGunYSensitivity(float newSensitivity)
    // {
    //     PlayerOptions.SetGunYSensitivity(newSensitivity);
    //     OnGunSensitivityUpdated?.Invoke(
    //         this,
    //         new Vector2(PlayerOptions.GetGunXSensitivity(), newSensitivity)
    //     );
    // }

    public void SetBulletSensitivity(float newSensitivity)
    {
        newSensitivity = newSensitivity / SLIDER_MOD;
        PlayerOptions.SetBulletXSensitivity(newSensitivity);
        OnBulletSensitivityUpdated?.Invoke(
            this,
            newSensitivity
        //new Vector2(newSensitivity, PlayerOptions.GetBulletYSensitivity())
        );
    }

    public void DisableFocus()
    {
        ToggleFocusControl(false);
    }

    public void ToggleFocusControl(bool toggle)
    {
        focusSettingState = toggle;
        PlayerOptions.SetFocusSetting(focusSettingState);

        UpdateFocusControlUI();
        OnFocusUpdated?.Invoke(this, focusSettingState);
    }

    private void UpdateFocusControlUI()
    {
        if (focusSettingState)
        {
            focusHoldButton.color = Color.gray;
            focusToggleButton.color = Color.white;
        }
        else
        {
            focusHoldButton.color = Color.white;
            focusToggleButton.color = Color.grey;
        }
    }

    public void ToggleVSync()
    {
        vSyncState = !vSyncState;

        if (vSyncState)
        {
            vsyncButtonImage.color = Color.white;
        }
        else
        {
            vsyncButtonImage.color = Color.gray;
        }

        PlayerOptions.SetVSync(vSyncState);

        OnVSyncUpdated?.Invoke(this, vSyncState);
    }

    // public void SetBulletYSensitivity(float newSensitivity)
    // {
    //     PlayerOptions.SetBulletYSensitivity(newSensitivity);
    //     OnBulletSensitivityUpdated?.Invoke(
    //         this,
    //         new Vector2(PlayerOptions.GetBulletXSensitivity(), newSensitivity)
    //     );
    // }
}
