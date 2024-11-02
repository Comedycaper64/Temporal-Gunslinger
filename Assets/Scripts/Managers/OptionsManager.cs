using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
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

    private void Awake()
    {
        masterSlider.value = PlayerOptions.GetMasterVolume();
        musicSlider.value = PlayerOptions.GetMusicVolume();
        sfxSlider.value = PlayerOptions.GetSFXVolume();
        voiceSlider.value = PlayerOptions.GetVoiceVolume();

        gunSensitivitySlider.value = PlayerOptions.GetGunSensitivity();
        //gunSensitivityYSlider.value = PlayerOptions.GetGunYSensitivity();
        bulletSensitivitySlider.value = PlayerOptions.GetBulletSensitivity();
        //bulletSensitivityYSlider.value = PlayerOptions.GetBulletYSensitivity();
    }

    public void SetMasterVolume(float newVolume)
    {
        PlayerOptions.SetMasterVolume(newVolume);
        OnMasterVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerOptions.SetMusicVolume(newVolume);
        OnMusicVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerOptions.SetSFXVolume(newVolume);
    }

    public void SetVoiceVolume(float newVolume)
    {
        PlayerOptions.SetVoiceVolume(newVolume);
    }

    public void SetGunSensitivity(float newSensitivity)
    {
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
