using UnityEngine;

public class TimeManager
{
    private static bool bTimeSlowed;
    private static bool bTimeTurbo;
    private static float normalTimeScale = 1f;
    private static float turboTimeScale = 2f;
    private static float slowTimeScale = 0.05f;
    private static float pausedTimeScale = 0f;

    public static void ResetTimeManager()
    {
        Time.timeScale = normalTimeScale;

        bTimeSlowed = false;
        bTimeTurbo = false;
    }

    public static void ToggleMenuTimePause(bool toggle)
    {
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
        if (bTimeSlowed)
        {
            timeScale *= slowTimeScale;
        }
        if (bTimeTurbo)
        {
            timeScale *= turboTimeScale;
        }
        Time.timeScale = timeScale;
    }

    public static void SetPausedTime()
    {
        Time.timeScale = pausedTimeScale;
        StopTime.TimeStopped();
    }

    public static void UnpauseTime()
    {
        Time.timeScale = normalTimeScale;
        UpdateTimeScale();
    }
}
