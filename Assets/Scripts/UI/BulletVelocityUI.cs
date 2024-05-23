using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletVelocityUI : MonoBehaviour
{
    private bool bActive;
    private float velocity = 0f;
    private float targetVelocity = 0f;
    private float displayVelocity = 0f;

    [SerializeField]
    private TextMeshProUGUI velocityText;

    [SerializeField]
    private Image velocityBar;

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
        ClearText();
        velocityBar.enabled = false;
    }

    private void Update()
    {
        velocity = Mathf.Lerp(velocity, targetVelocity, Time.unscaledDeltaTime);
        velocityBar.material.SetFloat("_DeltaVelocity", velocity);
    }

    private void ClearText()
    {
        velocityText.text = "";
    }

    private void UpdateText()
    {
        velocityText.text = "Speed: " + displayVelocity.ToString("0.0");
    }

    public void VelocityChanged(float newVelocity, float maxVelocity)
    {
        //this.maxVelocity = maxVelocity;
        displayVelocity = newVelocity;
        velocityBar.material.SetFloat("_MaxVelocity", maxVelocity);
        float invLerp = Mathf.InverseLerp(0f, maxVelocity, newVelocity);
        targetVelocity = Mathf.Lerp(-maxVelocity, 0f, invLerp);
        velocityBar.material.SetFloat("_Velocity", targetVelocity);
        if (!bActive)
        {
            return;
        }
        UpdateText();
    }

    public void ToggleUIActive(bool toggle)
    {
        bActive = toggle;
        velocityBar.enabled = toggle;
        if (!toggle)
        {
            ClearText();
        }
    }
}
