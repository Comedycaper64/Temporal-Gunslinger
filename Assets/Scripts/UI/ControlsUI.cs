using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject aimControlUI;

    [SerializeField]
    private GameObject shootControlUI;

    [SerializeField]
    private GameObject[] nonFocusUI;

    [SerializeField]
    private GameObject[] focusUI;

    private void Start()
    {
        DisableAllUI();
        PlayerController.OnPlayerStateChanged += ChangeUI;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStateChanged -= ChangeUI;
    }

    private void SetIdleUI()
    {
        DisableAllUI();
        aimControlUI.SetActive(true);
    }

    private void SetAimingUI()
    {
        DisableAllUI();
        shootControlUI.SetActive(true);
    }

    private void SetNonFocusUI()
    {
        DisableAllUI();
        foreach (GameObject ui in nonFocusUI)
        {
            ui.SetActive(true);
        }
    }

    private void SetFocusUI()
    {
        DisableAllUI();
        foreach (GameObject ui in focusUI)
        {
            ui.SetActive(true);
        }
    }

    public void DisableAllUI()
    {
        aimControlUI.SetActive(false);
        shootControlUI.SetActive(false);

        foreach (GameObject ui in nonFocusUI)
        {
            ui.SetActive(false);
        }
        foreach (GameObject ui in focusUI)
        {
            ui.SetActive(false);
        }
    }

    private void ChangeUI(object sender, int uIMode)
    {
        switch (uIMode)
        {
            case 0:
                DisableAllUI();
                break;
            case 1:
                SetIdleUI();
                break;
            case 2:
                SetAimingUI();
                break;
            case 3:
                SetNonFocusUI();
                break;
            case 4:
                SetFocusUI();
                break;
            default:
                break;
        }
    }
}
