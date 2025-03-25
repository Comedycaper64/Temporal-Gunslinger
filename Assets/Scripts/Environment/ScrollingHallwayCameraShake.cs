using Cinemachine;
using UnityEngine;

public class ScrollingHallwayCameraShake : MonoBehaviour
{
    private ScrollingHallway scrollingHallway;

    [SerializeField]
    private CinemachineVirtualCamera[] hallwayCameras;

    [SerializeField]
    private float noiseAmplitude = 1f;

    [SerializeField]
    private float noiseFrequency = 1f;

    [SerializeField]
    [NoiseSettingsProperty]
    private NoiseSettings cameraNoise;

    private void OnEnable()
    {
        scrollingHallway = GetComponent<ScrollingHallway>();

        scrollingHallway.OnScrollStart += SetCameraNoise;
    }

    private void OnDisable()
    {
        scrollingHallway.OnScrollStart -= SetCameraNoise;
    }

    private void SetCameraNoise()
    {
        foreach (CinemachineVirtualCamera camera in hallwayCameras)
        {
            CinemachineBasicMultiChannelPerlin noise =
                camera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_NoiseProfile = cameraNoise;
            noise.m_AmplitudeGain = noiseAmplitude;
            noise.m_FrequencyGain = noiseFrequency;
        }
    }
}
