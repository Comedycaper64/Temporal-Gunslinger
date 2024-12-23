using System;
using TMPro;
using UnityEngine;

public class DialogueAutoPlayUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI autoPlayImage;
    private static bool bAutoPlay = false;

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
        if (GameManager.bLevelActive)
        {
            return;
        }

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
