using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugFPSUI : MonoBehaviour
{
    public int MaxFrames = 60; //maximum frames to average over

    //private TextMeshProUGUI text;

    // [SerializeField]
    // private TextMeshProUGUI rrtext;
    //private static float lastFPSCalculated = 0f;
    private List<float> frameTimes = new List<float>();

    private void Awake()
    {
        Application.targetFrameRate = MaxFrames;

        //text = GetComponent<TextMeshProUGUI>();
        //lastFPSCalculated = 0f;
        frameTimes.Clear();
        //rrtext.text = "Hz: " + Screen.mainWindowDisplayInfo.refreshRate.value.ToString("0.0");

        SetupVsync(PlayerOptions.GetVSync());
    }

    private void OnEnable()
    {
        OptionsManager.OnVSyncUpdated += UpdateVSync;
    }

    private void OnDisable()
    {
        OptionsManager.OnVSyncUpdated -= UpdateVSync;
    }

    private void SetupVsync(bool toggle)
    {
        if (!toggle)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            double refreshRate = Screen.mainWindowDisplayInfo.refreshRate.value;

            if (refreshRate >= 240f)
            {
                QualitySettings.vSyncCount = 3;
            }
            else if (refreshRate >= 120f)
            {
                QualitySettings.vSyncCount = 2;
            }
            else
            {
                QualitySettings.vSyncCount = 1;
            }
        }
    }

    private void Update()
    {
        // AddFrame();
        // lastFPSCalculated = CalculateFPS();
        // text.text = "FPS: " + lastFPSCalculated.ToString("0.0");
    }

    private void AddFrame()
    {
        frameTimes.Add(Time.unscaledDeltaTime);
        if (frameTimes.Count > MaxFrames)
        {
            frameTimes.RemoveAt(0);
        }
    }

    private float CalculateFPS()
    {
        float newFPS = 0f;

        float totalTimeOfAllFrames = 0f;
        foreach (float frame in frameTimes)
        {
            totalTimeOfAllFrames += frame;
        }
        newFPS = ((float)(frameTimes.Count)) / totalTimeOfAllFrames;

        return newFPS;
    }

    private void UpdateVSync(object sender, bool toggle)
    {
        SetupVsync(toggle);
    }
}
