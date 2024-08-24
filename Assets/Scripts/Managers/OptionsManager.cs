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
    private Slider gunSensitivityXSlider;

    [SerializeField]
    private Slider gunSensitivityYSlider;

    [SerializeField]
    private Slider bulletSensitivityXSlider;

    [SerializeField]
    private Slider bulletSensitivityYSlider;

    public static EventHandler<float> OnMasterVolumeUpdated;
    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<Vector2> OnGunSensitivityUpdated;
    public static EventHandler<Vector2> OnBulletSensitivityUpdated;

    private void Awake()
    {
        masterSlider.value = PlayerOptions.GetMasterVolume();
        musicSlider.value = PlayerOptions.GetMusicVolume();
        sfxSlider.value = PlayerOptions.GetSFXVolume();

        gunSensitivityXSlider.value = PlayerOptions.GetGunXSensitivity();
        gunSensitivityYSlider.value = PlayerOptions.GetGunYSensitivity();
        bulletSensitivityXSlider.value = PlayerOptions.GetBulletXSensitivity();
        bulletSensitivityYSlider.value = PlayerOptions.GetBulletYSensitivity();
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

    public void SetGunXSensitivity(float newSensitivity)
    {
        PlayerOptions.SetGunXSensitivity(newSensitivity);
        OnGunSensitivityUpdated?.Invoke(
            this,
            new Vector2(newSensitivity, PlayerOptions.GetGunYSensitivity())
        );
    }

    public void SetGunYSensitivity(float newSensitivity)
    {
        PlayerOptions.SetGunYSensitivity(newSensitivity);
        OnGunSensitivityUpdated?.Invoke(
            this,
            new Vector2(PlayerOptions.GetGunXSensitivity(), newSensitivity)
        );
    }

    public void SetBulletXSensitivity(float newSensitivity)
    {
        PlayerOptions.SetBulletXSensitivity(newSensitivity);
        OnBulletSensitivityUpdated?.Invoke(
            this,
            new Vector2(newSensitivity, PlayerOptions.GetBulletYSensitivity())
        );
    }

    public void SetBulletYSensitivity(float newSensitivity)
    {
        PlayerOptions.SetBulletYSensitivity(newSensitivity);
        OnBulletSensitivityUpdated?.Invoke(
            this,
            new Vector2(PlayerOptions.GetBulletXSensitivity(), newSensitivity)
        );
    }
}
