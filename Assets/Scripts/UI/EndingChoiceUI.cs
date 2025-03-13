using System;
using UnityEngine;

public class EndingChoiceUI : LevelSelectorUI
{
    [SerializeField]
    private CinematicSO defyCinematic;

    [SerializeField]
    private CinematicSO handOverCinematic;

    [SerializeField]
    private CinematicManager cinematicManager;

    public static Action OnGameBeat;

    protected override void Update()
    {
        if (confirmation)
        {
            return;
        }

        if (selectorActive && pocketWatchGroup.alpha <= 1f)
        {
            pocketWatchGroup.alpha += previewFadeSpeed * Time.deltaTime;
        }
        else if (!selectorActive && pocketWatchGroup.alpha > 0f)
        {
            pocketWatchGroup.alpha -= previewFadeSpeed * Time.deltaTime;
            foreach (CanvasGroup preview in previews)
            {
                preview.alpha -= previewFadeSpeed * Time.deltaTime;
            }
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
            //RectTransform activeZone = eraZones[activePreviewIndex];

            activePreview.alpha += previewFadeSpeed * Time.deltaTime;
            // float zoneScale = Mathf.Clamp(
            //     activeZone.localScale.x + zoneSelectSpeed * Time.deltaTime,
            //     1f,
            //     1.25f
            // );
            //activeZone.localScale = new Vector3(zoneScale, zoneScale, zoneScale);

            foreach (CanvasGroup preview in previews)
            {
                if ((preview == activePreview) || (preview.alpha <= 0f))
                {
                    continue;
                }
                preview.alpha -= previewFadeSpeed * Time.deltaTime;
            }

            // foreach (RectTransform zone in eraZones)
            // {
            //     if ((zone == activeZone) || (zone.localScale.x <= 1f))
            //     {
            //         continue;
            //     }
            //     float cZoneScale = zone.localScale.x - zoneSelectSpeed * Time.deltaTime;
            //     zone.localScale = new Vector3(cZoneScale, cZoneScale, cZoneScale);
            // }

            if (activePreview.alpha >= 1f)
            {
                previewChange = false;
            }
        }
    }

    public override int GetMousedCanvasGroupIndex(float zRotation)
    {
        if ((zRotation >= 270f) || (zRotation < 90f))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void DefyTheReaper()
    {
        confirmation = false;
        ToggleLevelSelector(false);
    }

    public void HandOverThePocketwatch()
    {
        confirmation = false;
        ToggleLevelSelector(false);
        cinematicManager.PlayCinematic(handOverCinematic, StartCredits);
    }

    private void StartCredits()
    {
        OnGameBeat?.Invoke();
    }
}
