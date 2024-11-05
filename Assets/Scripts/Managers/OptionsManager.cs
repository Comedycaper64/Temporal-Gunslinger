using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    private const float SLIDER_MOD = 8f;

    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Slider voiceSlider;

    [SerializeField]
    private Slider gunSensitivitySlider;

    // [SerializeField]
    // private Slider gunSensitivityYSlider;

    [SerializeField]
    private Slider bulletSensitivitySlider;

    // [SerializeField]
    // private Slider bulletSensitivityYSlider;

    public static EventHandler<float> OnMasterVolumeUpdated;
    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<float> OnGunSensitivityUpdated;
    public static EventHandler<float> OnBulletSensitivityUpdated;

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
    }

    public void SetVoiceVolume(float newVolume)
    {
        newVolume = newVolume / SLIDER_MOD;
        PlayerOptions.SetVoiceVolume(newVolume);
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

    // public void SetBulletYSensitivity(float newSensitivity)
    // {
    //     PlayerOptions.SetBulletYSensitivity(newSensitivity);
    //     OnBulletSensitivityUpdated?.Invoke(
    //         this,
    //         new Vector2(PlayerOptions.GetBulletXSensitivity(), newSensitivity)
    //     );
    // }
}
