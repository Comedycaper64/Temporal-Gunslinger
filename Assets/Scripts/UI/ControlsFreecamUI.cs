using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsFreecamUI : MonoBehaviour
{
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
        }
        else
        {
            if (!uiActive)
            {
                uiActive = true;
                freeCamStartUI.SetActive(true);
            }
        }
    }

    private void ChangeFreeCamUI(object sender, bool toggle)
    {
        if (!uiActive)
        {
            return;
        }

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
