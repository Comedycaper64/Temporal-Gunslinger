using UnityEngine;

public class TimeManager
{
    private static bool bTimeSlowed;
    private static bool bTimeTurbo;
    private static bool bLevelLostPause = false;
    private static float normalTimeScale = 1f;
    private static float turboTimeScale = 2f;
    private static float slowTimeScale = 0.05f;
    private static float pausedTimeScale = 0f;

    public static void ResetTimeManager()
    {
        Time.timeScale = normalTimeScale;

        bTimeSlowed = false;
        bTimeTurbo = false;
        bLevelLostPause = false;
    }

    public static void ToggleMenuTimePause(bool toggle)
    {
        if (bLevelLostPause)
        {
            return;
        }

        if (toggle)
        {
            Time.timeScale = pausedTimeScale;
        }
        else
        {
            UnpauseTime();
        }
    }

    public static void SetNormalTime()
    {
        if (Time.timeScale == pausedTimeScale)
        {
            return;
        }

        Time.timeScale = normalTimeScale;
    }

    public static void SetSlowedTime(bool toggle)
    {
        bTimeSlowed = toggle;
        UpdateTimeScale();
    }

    public static void SetTurboTime(bool toggle)
    {
        bTimeTurbo = toggle;
        UpdateTimeScale();
    }

    public static void UpdateTimeScale()
    {
        if (Time.timeScale == pausedTimeScale)
        {
            return;
        }

        float timeScale = normalTimeScale;

        Time.fixedDeltaTime = 0.02f;
        if (bTimeSlowed)
        {
            timeScale *= slowTimeScale;
            Time.fixedDeltaTime *= slowTimeScale;
        }
        if (bTimeTurbo)
        {
            timeScale *= turboTimeScale;
            //Time.fixedDeltaTime *= turboTimeScale;
        }

        Time.timeScale = timeScale;
    }

    public static void SetPausedTime()
    {
        Time.timeScale = pausedTimeScale;
        bLevelLostPause = true;
        StopTime.TimeStopped();
    }

    public static void UnpauseTime()
    {
        Time.timeScale = normalTimeScale;
        bLevelLostPause = false;
        UpdateTimeScale();
    }
}
