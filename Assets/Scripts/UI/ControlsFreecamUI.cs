using UnityEngine;

public class ControlsFreecamUI : MonoBehaviour
{
    private bool controlUIToggle = false;
    private bool uiActive = false;

    [SerializeField]
    private GameObject freeCamStartUI;

    [SerializeField]
    private GameObject freeCamControlUI;

    private void Start()
    {
        PlayerController.OnPlayerStateChanged += ChangeUI;
        BulletFreeCamMovement.OnFreeCamToggle += ChangeFreeCamUI;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStateChanged -= ChangeUI;
        BulletFreeCamMovement.OnFreeCamToggle -= ChangeFreeCamUI;
    }

    private void ChangeUI(object sender, int uIMode)
    {
        if (uIMode < 3)
        {
            freeCamControlUI.SetActive(false);
            freeCamStartUI.SetActive(false);
            uiActive = false;

            if (uIMode > 0)
            {
                controlUIToggle = false;
            }
        }
        else
        {
            if (!uiActive)
            {
                uiActive = true;
                ToggleFreeCamUI(controlUIToggle);
            }
        }
    }

    private void ChangeFreeCamUI(object sender, bool toggle)
    {
        if (!uiActive)
        {
            return;
        }

        controlUIToggle = toggle;

        ToggleFreeCamUI(toggle);
    }

    private void ToggleFreeCamUI(bool toggle)
    {
        if (toggle)
        {
            freeCamControlUI.SetActive(true);
            freeCamStartUI.SetActive(false);
        }
        else
        {
            freeCamControlUI.SetActive(false);
            freeCamStartUI.SetActive(true);
        }
    }
}
