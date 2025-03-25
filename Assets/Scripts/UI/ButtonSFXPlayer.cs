using UnityEngine;

public class ButtonSFXPlayer : MonoBehaviour
{
    private float hoverSfxVolume = 1f;
    private float clickSfxVolume = 0.66f;

    [SerializeField]
    private AudioClip hoverSFX;

    [SerializeField]
    private AudioClip clickSFX;

    public void HoverSound()
    {
        AudioManager.PlaySFX(hoverSFX, hoverSfxVolume, 0, Camera.main.transform.position, false);
    }

    public void ClickSound()
    {
        AudioManager.PlaySFX(clickSFX, clickSfxVolume, 0, Camera.main.transform.position, false);
    }
}
