using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorUI : MonoBehaviour
{
    private bool previewChange = false;
    private float fadeSpeed = 5f;
    private bool selectorActive = false;
    private bool confirmation = false;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private Transform selectorArrow;

    [SerializeField]
    private CanvasGroup pocketWatchGroup;

    [SerializeField]
    private CanvasGroup[] previews;

    [SerializeField]
    private GameObject[] selectionConfirmation;

    [SerializeField]
    private CinematicSO endOfTutorialCinematic;
    private CanvasGroup activePreview;

    // private void Start()
    // {
    //     ToggleLevelSelector(true);
    // }

    private void Update()
    {
        if (confirmation)
        {
            return;
        }

        if (selectorActive && pocketWatchGroup.alpha <= 1f)
        {
            pocketWatchGroup.alpha += fadeSpeed * Time.deltaTime;
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
            float angleDeg = (180f / Mathf.PI) * angleRad;

            selectorArrow.eulerAngles = new Vector3(0f, 0f, angleDeg);
            //Debug.Log(selectorArrow.eulerAngles.z);

            CanvasGroup desiredCanvas = GetMousedCanvasGroup(selectorArrow.eulerAngles.z);

            if (desiredCanvas != activePreview)
            {
                previewChange = true;
                activePreview = desiredCanvas;
            }
        }

        if (previewChange)
        {
            activePreview.alpha += fadeSpeed * Time.deltaTime;

            foreach (CanvasGroup preview in previews)
            {
                if ((preview == activePreview) || (preview.alpha <= 0f))
                {
                    continue;
                }
                preview.alpha -= fadeSpeed * Time.deltaTime;
            }

            if (activePreview.alpha >= 1f)
            {
                previewChange = false;
            }
        }
    }

    public CanvasGroup GetMousedCanvasGroup(float zRotation)
    {
        if ((zRotation >= 330f) || (zRotation < 30f))
        {
            return previews[1];
        }
        else if (zRotation < 150f)
        {
            return previews[0];
        }
        else if (zRotation < 210f)
        {
            return previews[4];
        }
        else if (zRotation < 270f)
        {
            return previews[3];
        }
        else
        {
            return previews[2];
        }
    }

    private void ConfirmLevelSelection()
    {
        confirmation = true;
        selectionConfirmation[Array.IndexOf(previews, activePreview)].SetActive(true);
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
        SceneManager.LoadScene(1);
    }

    // public void ShowPreview(int previewIndex)
    // {
    //     activePreview = previews[previewIndex];

    //     previewChange = true;
    // }

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
