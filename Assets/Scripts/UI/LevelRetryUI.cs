using UnityEngine;

public class LevelRetryUI : MonoBehaviour
{
    private float dampVelocity1;
    private float dampVelocity2;
    private CanvasGroup canvasGroup;
    private float targetAlpha;
    private bool changeOpacity = false;
    private float uiScale = 1.4f;
    private float targetScale = 1f;

    private void Start()
    {
        GameManager.Instance.OnLevelLost += GameManager_OnLevelLost;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        transform.localScale = new Vector3(uiScale, uiScale, uiScale);
    }

    private void Update()
    {
        if (changeOpacity)
        {
            canvasGroup.alpha = Mathf.SmoothDamp(
                canvasGroup.alpha,
                targetAlpha,
                ref dampVelocity1,
                0.5f,
                Mathf.Infinity,
                Time.unscaledDeltaTime
            );

            float newScale = Mathf.SmoothDamp(
                transform.localScale.x,
                targetScale,
                ref dampVelocity2,
                0.5f,
                Mathf.Infinity,
                Time.unscaledDeltaTime
            );

            transform.localScale = new Vector3(newScale, newScale, newScale);

            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
                changeOpacity = false;
            }
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelLost -= GameManager_OnLevelLost;
    }

    private void GameManager_OnLevelLost(object sender, bool e)
    {
        changeOpacity = true;
        if (e)
        {
            targetAlpha = 1f;
            targetScale = 1f;
        }
        else
        {
            targetAlpha = 0f;
            targetScale = uiScale;
        }
    }
}
