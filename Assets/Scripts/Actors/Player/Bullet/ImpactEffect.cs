using UnityEngine;

public class ImpactEffect : RewindableMovement
{
    private float dampVelocity;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float newAlpha = Mathf.SmoothDamp(spriteRenderer.color.a, 0f, ref dampVelocity, 5f);
        spriteRenderer.color = new Color(1f, 1f, 1f, newAlpha);

        if (newAlpha <= 0f)
        {
            Destroy(this);
        }
    }
}
