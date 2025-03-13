using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{
    protected bool previewChange = false;
    protected float previewFadeSpeed = 5f;
    protected float zoneSelectSpeed = 1.25f;
    protected bool selectorActive = false;
    protected bool confirmation = false;

    // [SerializeField]
    // private int nextLevelBuildIndex;

    [SerializeField]
    protected RectTransform canvas;

    [SerializeField]
    protected Transform selectorArrow;

    [SerializeField]
    protected CanvasGroup pocketWatchGroup;

    [SerializeField]
    protected CanvasGroup[] previews;

    [SerializeField]
    protected RectTransform[] eraZones;

    [SerializeField]
    protected GameObject[] selectionConfirmation;

    [SerializeField]
    protected AudioClip eraChangeSFX;

    protected int activePreviewIndex;

    protected virtual void Update()
    {
        if (confirmation)
        {
            return;
        }

        if (selectorActive && pocketWatchGroup.alpha <= 1f)
        {
            pocketWatchGroup.alpha += previewFadeSpeed * Time.deltaTime;
        }

        if (selectorActive)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0f;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas,
                mousePosition,
                null,
                out Vector3 localPoint
            );

            float angleRad = Mathf.Atan2(
                localPoint.y - selectorArrow.position.y,
                localPoint.x - selectorArrow.position.x
            );
            float angleDeg = 180f / Mathf.PI * angleRad;

            selectorArrow.eulerAngles = new Vector3(0f, 0f, angleDeg);

            int desiredCanvasIndex = GetMousedCanvasGroupIndex(selectorArrow.eulerAngles.z);

            if (desiredCanvasIndex != activePreviewIndex)
            {
                previewChange = true;
                activePreviewIndex = desiredCanvasIndex;
                AudioManager.PlaySFX(eraChangeSFX, 0.25f, 0, Camera.main.transform.position);
            }
        }

        if (previewChange)
        {
            CanvasGroup activePreview = previews[activePreviewIndex];
            RectTransform activeZone = eraZones[activePreviewIndex];

            activePreview.alpha += previewFadeSpeed * Time.deltaTime;
            float zoneScale = Mathf.Clamp(
                activeZone.localScale.x + zoneSelectSpeed * Time.deltaTime,
                1f,
                1.25f
            );
            activeZone.localScale = new Vector3(zoneScale, zoneScale, zoneScale);

            foreach (CanvasGroup preview in previews)
            {
                if ((preview == activePreview) || (preview.alpha <= 0f))
                {
                    continue;
                }
                preview.alpha -= previewFadeSpeed * Time.deltaTime;
            }

            foreach (RectTransform zone in eraZones)
            {
                if ((zone == activeZone) || (zone.localScale.x <= 1f))
                {
                    continue;
                }
                float cZoneScale = zone.localScale.x - zoneSelectSpeed * Time.deltaTime;
                zone.localScale = new Vector3(cZoneScale, cZoneScale, cZoneScale);
            }

            if (activePreview.alpha >= 1f)
            {
                previewChange = false;
            }
        }
    }

    public virtual int GetMousedCanvasGroupIndex(float zRotation)
    {
        if ((zRotation >= 330f) || (zRotation < 30f))
        {
            return 2;
        }
        else if (zRotation < 90f)
        {
            return 1;
        }
        else if (zRotation < 210f)
        {
            return 0;
        }
        else if (zRotation < 270f)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }

    private void ConfirmLevelSelection()
    {
        confirmation = true;
        selectionConfirmation[activePreviewIndex].SetActive(true);
    }

    public void CancelConfirmation()
    {
        confirmation = false;
    }

    public void LevelSelected()
    {
        //CinematicManager.Instance.PlayCinematic(endOfTutorialCinematic, LoadNextLevel);
        GameManager.Instance.EndLevel(transform);
    }

    public void EndTutorial()
    {
        TutorialGameManager tutorialGameManager = GameManager.Instance as TutorialGameManager;
        tutorialGameManager.EndTutorial();
    }

    public void LoadNextLevel()
    {
        //SceneManager.LoadScene(nextLevelBuildIndex);
    }

    public void ToggleLevelSelector(bool toggle)
    {
        selectorActive = toggle;
        if (toggle)
        {
            InputManager.Instance.OnShootAction += InputManager_OnShoot;
        }
        else
        {
            InputManager.Instance.OnShootAction -= InputManager_OnShoot;
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.OnShootAction -= InputManager_OnShoot;
    }

    private void InputManager_OnShoot()
    {
        if (confirmation == false)
        {
            ConfirmLevelSelection();
        }
    }
}
