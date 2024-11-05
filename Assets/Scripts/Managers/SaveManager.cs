using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    private const int LEVEL_PROGRESS_DEF = 0;
    private const string LEVEL_PROGRESS = "LvlProgress";

    public static int GetLevelProgress()
    {
        return 13;

        // if (!PlayerPrefs.HasKey(LEVEL_PROGRESS))
        // {
        //     return LEVEL_PROGRESS_DEF;
        // }
        // else
        // {
        //     return PlayerPrefs.GetInt(LEVEL_PROGRESS);
        // }
    }

    public static void SetLevelProgress(int newProgress)
    {
        if (
            !PlayerPrefs.HasKey(LEVEL_PROGRESS)
            || (newProgress > PlayerPrefs.GetInt(LEVEL_PROGRESS))
        )
        {
            PlayerPrefs.SetInt(LEVEL_PROGRESS, newProgress);
        }
    }
}
