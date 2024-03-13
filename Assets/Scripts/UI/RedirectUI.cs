using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedirectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject redirectUI;

    [SerializeField]
    private TextMeshProUGUI redirectText;

    private void Awake()
    {
        RedirectManager.OnRedirectsChanged += UpdateText;
        RedirectManager.OnRedirectUIActive += ToggleUI;
    }

    private void ToggleUI(object sender, bool e)
    {
        redirectUI.SetActive(e);
    }

    private void UpdateText(object sender, int e)
    {
        redirectText.text = e.ToString();
    }

    private void OnDisable()
    {
        RedirectManager.OnRedirectsChanged -= UpdateText;
        RedirectManager.OnRedirectUIActive -= ToggleUI;
    }
}
