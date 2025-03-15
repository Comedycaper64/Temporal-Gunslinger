using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugFPSUI : MonoBehaviour
{
    public int MaxFrames = 60; //maximum frames to average over
    private TextMeshProUGUI text;
    private static float lastFPSCalculated = 0f;
    private List<float> frameTimes = new List<float>();

    private void Awake()
    {
        Application.targetFrameRate = MaxFrames;

        text = GetComponent<TextMeshProUGUI>();
        lastFPSCalculated = 0f;
        frameTimes.Clear();
    }

    private void Update()
    {
        addFrame();
        lastFPSCalculated = calculateFPS();
        text.text = "FPS: " + lastFPSCalculated.ToString("0.0");
    }

    private void addFrame()
    {
        frameTimes.Add(Time.unscaledDeltaTime);
        if (frameTimes.Count > MaxFrames)
        {
            frameTimes.RemoveAt(0);
        }
    }

    private float calculateFPS()
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
}
