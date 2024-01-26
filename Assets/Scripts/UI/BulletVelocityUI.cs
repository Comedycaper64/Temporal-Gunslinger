using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletVelocityUI : MonoBehaviour
{
    private float velocity;

    [SerializeField]
    private TextMeshProUGUI velocityText;

    public static BulletVelocityUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one BulletVelocityUI! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void UpdateText()
    {
        velocityText.text = "Velocity = " + velocity.ToString("0.0");
    }

    public void VelocityChanged(float newVelocity)
    {
        velocity = newVelocity;
        UpdateText();
    }
}
