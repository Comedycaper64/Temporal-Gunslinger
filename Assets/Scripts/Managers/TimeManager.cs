using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    private static bool bTimeSlowed;
    private static float normalTimeScale = 1f;
    private static float slowTimeScale = 0.5f;
    private static float pausedTimeScale = 0f;

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
