using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueAutoPlayUI : MonoBehaviour
{
    [SerializeField]
    private Image autoPlayImage;
    private static bool bAutoPlay = true;

    public static EventHandler<bool> OnAutoPlayToggle;

    private void Start()
    {
        SetAutoPlay(bAutoPlay);
        InputManager.Instance.OnFocusAction += AutoPlayButtonPress;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnFocusAction -= AutoPlayButtonPress;
    }

    private void SetAutoPlay(bool toggle)
    {
        OnAutoPlayToggle?.Invoke(this, toggle);
        if (toggle)
        {
            autoPlayImage.color = Color.white;
        }
        else
        {
            autoPlayImage.color = Color.grey;
        }
    }

    public void AutoPlayButtonPress()
    {
        bAutoPlay = !bAutoPlay;
        SetAutoPlay(bAutoPlay);
    }
}