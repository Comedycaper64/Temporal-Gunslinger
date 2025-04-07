using UnityEngine;

public class SaveManager
{
    private const int LEVEL_PROGRESS_DEF = 0;
    private const string LEVEL_PROGRESS = "LvlProgress";

    public static int GetLevelProgress()
    {
        //return 19;

        if (!PlayerPrefs.HasKey(LEVEL_PROGRESS))
        {
            //(temp for starting at later level)
            SetLevelProgress(LEVEL_PROGRESS_DEF);

            return LEVEL_PROGRESS_DEF;
        }
        else
        {
            return PlayerPrefs.GetInt(LEVEL_PROGRESS);
        }
    }

    public static void SetLevelProgress(int newProgress)
    {
        if (
            !PlayerPrefs.HasKey(LEVEL_PROGRESS)
            || (newProgress > PlayerPrefs.GetInt(LEVEL_PROGRESS))
        )
        {
            if (newProgress > 24)
            {
                newProgress = 24;
            }

            PlayerPrefs.SetInt(LEVEL_PROGRESS, newProgress);
        }
    }

    public static void ResetProgress()
    {
        PlayerPrefs.SetInt(LEVEL_PROGRESS, 0);
    }
}
