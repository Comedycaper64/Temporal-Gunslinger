using UnityEngine;

public class PlayerOptions
{
    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string VOICE_VOLUME = "VoiceVolume";
    private const string GUN_SENSITIVITY = "GunSens";
    private const string BULLET_SENSITIVITY = "BulletSens";
    private const string DIALOGUE_AUTOPLAY = "AutoPlay";
    private const string VSYNC = "VSync";

    private static float MASTER_VOLUME_DEF = 0.5f;
    private static float MUSIC_VOLUME_DEF = 1f;
    private static float SFX_VOLUME_DEF = 1f;
    private static float VOICE_VOLUME_DEF = 1f;

    private static float GUN_SENSITIVITY_DEF = 1f;
    private static float BULLET_SENSITIVITY_DEF = 1f;

    private static bool DIALOGUE_AUTOPLAY_DEF = false;
    private static bool VSYNC_DEF = true;

    public static void SetMasterVolume(float newVolume)
    {
        //MASTER_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(MASTER_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetMusicVolume(float newVolume)
    {
        //MUSIC_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float newVolume)
    {
        // SFX_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(SFX_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetVoiceVolume(float newVolume)
    {
        //VOICE_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(VOICE_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetGunXSensitivity(float newSensitivity)
    {
        //GUN_SENSITIVITY_DEF = newSensitivity;
        PlayerPrefs.SetFloat(GUN_SENSITIVITY, newSensitivity);
        PlayerPrefs.Save();
    }

    public static void SetBulletXSensitivity(float newSensitivity)
    {
        //BULLET_SENSITIVITY_DEF = newSensitivity;
        PlayerPrefs.SetFloat(BULLET_SENSITIVITY, newSensitivity);
        PlayerPrefs.Save();
    }

    public static void SetDialogueAutoPlay(bool toggle)
    {
        int autoPlay = 0;
        if (toggle)
        {
            autoPlay = 1;
        }

        PlayerPrefs.SetInt(DIALOGUE_AUTOPLAY, autoPlay);
    }

    public static void SetVSync(bool toggle)
    {
        int vSync = 0;
        if (toggle)
        {
            vSync = 1;
        }

        PlayerPrefs.SetInt(VSYNC, vSync);
    }

    public static float GetMasterVolume()
    {
        if (!PlayerPrefs.HasKey(MASTER_VOLUME))
        {
            return MASTER_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MASTER_VOLUME);
        }
    }

    public static float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return MUSIC_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
    }

    public static float GetSFXVolume()
    {
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            return SFX_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
    }

    public static float GetVoiceVolume()
    {
        if (!PlayerPrefs.HasKey(VOICE_VOLUME))
        {
            return VOICE_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(VOICE_VOLUME);
        }
    }

    public static float GetGunSensitivity()
    {
        if (!PlayerPrefs.HasKey(GUN_SENSITIVITY))
        {
            return GUN_SENSITIVITY_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(GUN_SENSITIVITY);
        }
    }

    public static float GetBulletSensitivity()
    {
        if (!PlayerPrefs.HasKey(BULLET_SENSITIVITY))
        {
            return BULLET_SENSITIVITY_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(BULLET_SENSITIVITY);
        }
    }

    public static bool GetDialogueAutoPlay()
    {
        if (!PlayerPrefs.HasKey(DIALOGUE_AUTOPLAY))
        {
            return DIALOGUE_AUTOPLAY_DEF;
        }
        else
        {
            bool autoPlay = false;
            if (PlayerPrefs.GetInt(DIALOGUE_AUTOPLAY) == 1)
            {
                autoPlay = true;
            }

            return autoPlay;
        }
    }

    public static bool GetVSync()
    {
        if (!PlayerPrefs.HasKey(VSYNC))
        {
            return VSYNC_DEF;
        }
        else
        {
            bool vSync = false;
            if (PlayerPrefs.GetInt(VSYNC) == 1)
            {
                vSync = true;
            }

            return vSync;
        }
    }
}
