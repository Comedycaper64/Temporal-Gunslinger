public class PlayerOptions
{
    private static float MASTER_VOLUME = 0.5f;
    private static float MUSIC_VOLUME = 1f;
    private static float SFX_VOLUME = 1f;

    private static float GUN_SENSITIVITY = 1f;

    //private static float GUN_SENSITIVITY_Y = 1f;
    private static float BULLET_SENSITIVITY = 1f;

    //private static float BULLET_SENSITIVITY_Y = 1f;

    public static void SetMasterVolume(float newVolume)
    {
        MASTER_VOLUME = newVolume;
        //Save in Settings File
    }

    public static void SetMusicVolume(float newVolume)
    {
        MUSIC_VOLUME = newVolume;
        //Save in Settings File
    }

    public static void SetSFXVolume(float newVolume)
    {
        SFX_VOLUME = newVolume;
        //Save in Settings File
    }

    public static void SetGunXSensitivity(float newSensitivity)
    {
        GUN_SENSITIVITY = newSensitivity;
        //Save in Settings File
    }

    // public static void SetGunYSensitivity(float newSensitivity)
    // {
    //     GUN_SENSITIVITY_Y = newSensitivity;
    //     //Save in Settings File
    // }

    public static void SetBulletXSensitivity(float newSensitivity)
    {
        BULLET_SENSITIVITY = newSensitivity;
        //Save in Settings File
    }

    // public static void SetBulletYSensitivity(float newSensitivity)
    // {
    //     BULLET_SENSITIVITY_Y = newSensitivity;
    //     //Save in Settings File
    // }

    public static float GetMasterVolume()
    {
        return MASTER_VOLUME;
    }

    public static float GetMusicVolume()
    {
        return MUSIC_VOLUME;
    }

    public static float GetSFXVolume()
    {
        return SFX_VOLUME;
    }

    public static float GetGunSensitivity()
    {
        return GUN_SENSITIVITY;
    }

    // public static float GetGunYSensitivity()
    // {
    //     return GUN_SENSITIVITY_Y;
    // }

    public static float GetBulletSensitivity()
    {
        return BULLET_SENSITIVITY;
    }

    // public static float GetBulletYSensitivity()
    // {
    //     return BULLET_SENSITIVITY_Y;
    // }
}
