using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletVelocityUI : MonoBehaviour
{
    private bool bActive;
    private int activeSpeedCategory = -1;
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

    [SerializeField]
    private Transform speedCategories;

    [SerializeField]
    private Image[] speedIcons;

    [SerializeField]
    private TextMeshProUGUI[] speedText;

    [SerializeField]
    private Color activeIconColour;

    [SerializeField]
    private Color inactiveIconColour;

    [SerializeField]
    private float inactiveIconScale;

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
        velocity = Mathf.SmoothDamp(
            velocity,
            targetVelocity,
            ref dampRef,
            smoothTime,
            Mathf.Infinity,
            Time.unscaledDeltaTime
        );
        velocityBar.material.SetFloat("_DeltaVelocity", velocity);

        SetSpeedCategoriesPos(velocity);
        SetSpeedIcons(velocity);
    }

    private void SetSpeedIcons(float velocity)
    {
        int index = 2;
        if (velocity < 175f)
        {
            index = 0;
        }
        else if (velocity < 350f)
        {
            index = 1;
        }

        if (index != activeSpeedCategory)
        {
            SetNewActiveSpeedIcon(index);
        }
    }

    private void SetNewActiveSpeedIcon(int index)
    {
        for (int i = 0; i < speedIcons.Length; i++)
        {
            if (i == index)
            {
                speedIcons[i].color = activeIconColour;
                speedText[i].color = activeIconColour;
                speedIcons[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                speedIcons[i].color = inactiveIconColour;
                speedText[i].color = inactiveIconColour;
                speedIcons[i].transform.localScale = new Vector3(
                    inactiveIconScale,
                    inactiveIconScale,
                    inactiveIconScale
                );
            }
        }
    }

    private void SetSpeedCategoriesPos(float velocity)
    {
        float speedLerp = Mathf.InverseLerp(0f, 600f, velocity);
        float xPosition = Mathf.Lerp(100f, -100f, speedLerp);

        speedCategories.localPosition = new Vector3(
            xPosition,
            speedCategories.localPosition.y,
            speedCategories.localPosition.z
        );
    }

    private void ClearText()
    {
        velocityText.text = "";
    }

    private void UpdateText()
    {
        velocityText.text = displayVelocity.ToString("0.0") + " m/s";
    }

    public void VelocityChanged(float newVelocity, float maxVelocity, float lowVelocity)
    {
        //this.maxVelocity = maxVelocity;
        displayVelocity = newVelocity;
        targetVelocity = newVelocity;
        velocityBar.material.SetFloat("_MaxVelocity", maxVelocity);

        velocityBar.material.SetFloat("_PulseThreshold", lowVelocity);

        velocityBar.material.SetFloat("_Velocity", targetVelocity);

        if (!bActive)
        {
            return;
        }
        UpdateText();
    }

    // public void ToggleLowVelocity(bool toggle)
    // {
    //     lowVelocityUI.SetActive(toggle);
    // }

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
