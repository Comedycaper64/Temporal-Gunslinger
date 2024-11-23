using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorUI : MonoBehaviour
{
    private bool previewChange = false;
    private float previewFadeSpeed = 5f;
    private float zoneSelectSpeed = 1.25f;
    private bool selectorActive = false;
    private bool confirmation = false;

    [SerializeField]
    private int nextLevelBuildIndex;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private Transform selectorArrow;

    [SerializeField]
    private CanvasGroup pocketWatchGroup;

    [SerializeField]
    private CanvasGroup[] previews;

    [SerializeField]
    private RectTransform[] eraZones;

    [SerializeField]
    private GameObject[] selectionConfirmation;

    [SerializeField]
    private AudioClip eraChangeSFX;

    [SerializeField]
    private CinematicSO endOfTutorialCinematic;
    private int activePreviewIndex;

    private void Update()
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

    public int GetMousedCanvasGroupIndex(float zRotation)
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

    public void EndTutorial()
    {
        CinematicManager.Instance.PlayCinematic(endOfTutorialCinematic, LoadNextLevel);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelBuildIndex);
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

    private void InputManager_OnShoot()
    {
        if (confirmation == false)
        {
            ConfirmLevelSelection();
        }
    }
}
