using UnityEngine;
using UnityEngine.UI;

public class DangerMarkerUI : MonoBehaviour
{
    [SerializeField]
    private Image deathImage;

    public void SetDeathIcon(Sprite icon)
    {
        deathImage.sprite = icon;
        deathImage.enabled = true;
    }

    public void ClearMarker()
    {
        deathImage.sprite = null;
        deathImage.enabled = false;
    }
}
