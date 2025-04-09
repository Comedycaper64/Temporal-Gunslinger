using UnityEngine;

public class ImpactEffect : RewindableMovement
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float newAlpha = spriteRenderer.color.a - (GetUnscaledSpeed() * Time.deltaTime);
        spriteRenderer.color = new Color(1f, 1f, 1f, newAlpha);

        if ((newAlpha <= 0f) || (newAlpha > 1f))
        {
            ToggleMovement(false);
            gameObject.SetActive(false);
        }
    }

    public void ResetEffect()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
