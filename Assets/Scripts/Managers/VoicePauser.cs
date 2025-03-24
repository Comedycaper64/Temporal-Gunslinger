public class VoicePauser : SFXPauser
{
    protected override void OnEnable()
    {
        GameManager.OnGameStateChange += ToggleAudioSource;

        if (loopingSFX)
        {
            OptionsManager.OnMasterVolumeUpdated += UpdateVolume;
            OptionsManager.OnVoiceVolumeUpdated += UpdateVolume;
        }
    }

    protected override void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleAudioSource;

        if (loopingSFX)
        {
            OptionsManager.OnMasterVolumeUpdated -= UpdateVolume;
            OptionsManager.OnVoiceVolumeUpdated -= UpdateVolume;
        }
    }

    protected override void PlaySFX()
    {
        sfxAudioSource.volume = PlayerOptions.GetVoiceVolume() * sfxVolume;
    }

    protected override void UpdateVolume(object sender, float e)
    {
        sfxAudioSource.volume = PlayerOptions.GetVoiceVolume() * sfxVolume;
    }
}
