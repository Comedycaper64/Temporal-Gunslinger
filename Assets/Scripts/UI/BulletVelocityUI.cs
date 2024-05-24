using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletVelocityUI : MonoBehaviour
{
    private bool bActive;
    private float dampRef = 0f;
    private float smoothTime = 1f;
    private float velocity = 0f;
    private float targetVelocity = 0f;
    private float displayVelocity = 0f;

    private CanvasGroup canvasGroup;

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
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        //velocity = Mathf.Lerp(velocity, targetVelocity, Time.unscaledDeltaTime);
        velocity = Mathf.SmoothDamp(velocity, targetVelocity, ref dampRef, smoothTime);
        velocityBar.material.SetFloat("_DeltaVelocity", velocity);
    }

    private void ClearText()
    {
        velocityText.text = "";
    }

    private void UpdateText()
    {
        velocityText.text = displayVelocity.ToString("0.0");
    }

    public void VelocityChanged(float newVelocity, float maxVelocity)
    {
        //this.maxVelocity = maxVelocity;
        displayVelocity = newVelocity;
        velocityBar.material.SetFloat("_MaxVelocity", maxVelocity);
        float invLerp = Mathf.InverseLerp(0f, maxVelocity, newVelocity);
        targetVelocity = Mathf.Lerp(-maxVelocity + maxVelocity * 0.25f, 0f, invLerp);
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
        if (toggle)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
            ClearText();
        }
    }
}
