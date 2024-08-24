using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class RedirectUI : MonoBehaviour
{
    [SerializeField]
    private Color noCoinsColour;

    [SerializeField]
    private Color coinChangeColour;

    [SerializeField]
    private Gradient noCoinsGradient;

    [SerializeField]
    private Gradient coinChangeGradient;

    private MMF_Player flashPlayer;
    private CanvasGroupFader canvasFader;

    [SerializeField]
    private TextMeshProUGUI redirectText;

    private void Awake()
    {
        RedirectManager.OnRedirectsChanged += UpdateText;
        RedirectManager.OnRedirectUIActive += ToggleUI;
        RedirectManager.OnRedirectFailed += FlashNoCoins;

        flashPlayer = GetComponent<MMF_Player>();
        canvasFader = GetComponent<CanvasGroupFader>();
        canvasFader.SetCanvasGroupAlpha(0f);
    }

    private void FlashNoCoins()
    {
        flashPlayer.GetFeedbackOfType<MMF_TMPColor>().DestinationColor = noCoinsColour;
        flashPlayer.GetFeedbackOfType<MMF_Image>().ColorOverTime = noCoinsGradient;
        PlayFeedback();
    }

    private void ToggleUI(object sender, bool e)
    {
        canvasFader.ToggleFade(e);
    }

    private void UpdateText(object sender, int e)
    {
        redirectText.text = "x " + e.ToString();
        flashPlayer.GetFeedbackOfType<MMF_TMPColor>().DestinationColor = coinChangeColour;
        flashPlayer.GetFeedbackOfType<MMF_Image>().ColorOverTime = coinChangeGradient;
        PlayFeedback();
    }

    private void PlayFeedback()
    {
        flashPlayer.PlayFeedbacks();
    }

    private void OnDisable()
    {
        RedirectManager.OnRedirectsChanged -= UpdateText;
        RedirectManager.OnRedirectUIActive -= ToggleUI;
        RedirectManager.OnRedirectFailed -= FlashNoCoins;
    }
}
