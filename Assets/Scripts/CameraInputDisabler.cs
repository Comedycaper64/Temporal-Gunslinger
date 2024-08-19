using Cinemachine;
using UnityEngine;

public class CameraInputDisabler : MonoBehaviour
{
    [SerializeField]
    private CinemachineInputProvider cinemachineInput;

    private void OnEnable()
    {
        PauseMenuUI.OnPauseToggled += ToggleInput;
    }

    private void OnDisable()
    {
        PauseMenuUI.OnPauseToggled -= ToggleInput;
    }

    private void ToggleInput(object sender, bool toggle)
    {
        cinemachineInput.enabled = !toggle;
    }
}
