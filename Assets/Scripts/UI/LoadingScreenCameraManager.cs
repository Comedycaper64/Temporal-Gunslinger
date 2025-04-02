using UnityEngine;

public class LoadingScreenCameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreenCamera;

    private void OnEnable()
    {
        CinematicManagerUI.OnFadeStart += ToggleCameraStart;
        CinematicManagerUI.OnFadeEnd += ToggleCameraEnd;
    }

    private void OnDisable()
    {
        CinematicManagerUI.OnFadeStart -= ToggleCameraStart;
        CinematicManagerUI.OnFadeEnd -= ToggleCameraEnd;
    }

    private void ToggleCameraStart(object sender, bool toggle)
    {
        if (toggle)
        {
            loadingScreenCamera.SetActive(true);
        }
    }

    private void ToggleCameraEnd(object sender, bool toggle)
    {
        if (!toggle)
        {
            loadingScreenCamera.SetActive(false);
        }
    }
}
