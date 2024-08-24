using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshairUI;

    private void Start()
    {
        FocusManager.OnFocusToggle += ToggleUI;
        PlayerGun.OnAimGun += ToggleUI;
        ToggleUI(this, false);
    }

    private void OnDisable()
    {
        FocusManager.OnFocusToggle -= ToggleUI;
        PlayerGun.OnAimGun -= ToggleUI;
    }

    private void ToggleUI(object sender, bool toggle)
    {
        crosshairUI.SetActive(toggle);
    }
}
