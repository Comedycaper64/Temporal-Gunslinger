using System;
using UnityEngine;
using UnityEngine.UI;

public class PocketwatchUI : RewindableMovement
{
    private bool uiActive = false;
    private float currentPocketwatchTime = 0f;
    private int deathMarkerIndex = 0;
    public static float pocketwatchTime = 0f;

    [SerializeField]
    private Transform pockewatchHand;

    [SerializeField]
    private Transform markerPlacerHand;

    [SerializeField]
    private Transform markerPlacerTip;

    [SerializeField]
    private Image[] deathMarkers;

    private CanvasGroupFader canvasGroupFader;

    public Action OnShowUI;
    public Action OnPocketwatchReset;

    protected override void OnEnable()
    {
        base.OnEnable();
        canvasGroupFader = GetComponent<CanvasGroupFader>();
        canvasGroupFader.SetCanvasGroupAlpha(0f);
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
        //RewindManager.OnRewindToStart += RewindManager_OnRewindToStart;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
        //RewindManager.OnRewindToStart -= RewindManager_OnRewindToStart;
    }

    private void Update()
    {
        pocketwatchTime = currentPocketwatchTime;
        if (uiActive)
        {
            float newZDegree = pockewatchHand.eulerAngles.z + (-GetSpeed() * Time.deltaTime);
            pockewatchHand.eulerAngles = new Vector3(0, 0, newZDegree);
            currentPocketwatchTime = Mathf.Abs(360f - newZDegree) / Mathf.Abs(GetStartSpeed());
        }
    }

    private void ToggleUI(bool toggle)
    {
        uiActive = toggle;
        canvasGroupFader.ToggleFade(toggle);

        currentPocketwatchTime = 0f;
    }

    public void ClearDeathTimes()
    {
        deathMarkerIndex = 0;
        foreach (Image marker in deathMarkers)
        {
            marker.sprite = null;
            marker.gameObject.SetActive(false);
        }
    }

    public void SetDeathTime(float newDeathTime, Sprite deathIcon)
    {
        if (!uiActive)
        {
            return;
        }

        if (deathMarkerIndex >= deathMarkers.Length)
        {
            return;
        }

        deathMarkers[deathMarkerIndex].sprite = deathIcon;
        deathMarkers[deathMarkerIndex].gameObject.SetActive(true);

        float handAngle = Mathf.Clamp(-Mathf.Abs(GetStartSpeed()) * newDeathTime, -1000f, 1000f);

        markerPlacerHand.eulerAngles = new Vector3(0, 0, handAngle);

        deathMarkers[deathMarkerIndex].transform.position = markerPlacerTip.position;

        deathMarkerIndex++;

        // if (newDeathTime < 0f)
        // {
        //     deathMarker.gameObject.SetActive(false);
        // }
        // else
        // {
        //     //Debug.Log("Death Time: " + newDeathTime)
        //     //deathTime = currentPocketwatchTime + newDeathTime;
        //     deathMarker.gameObject.SetActive(true);
        //     markerPlacerHand.eulerAngles = new Vector3(
        //         0,
        //         0,
        //         -Mathf.Abs(GetStartSpeed()) * newDeathTime
        //     );

        //     deathMarker.position = markerPlacerTip.position;
        // }
    }

    public float GetCurrentPocketwatchTime()
    {
        return currentPocketwatchTime;
    }

    private void GameManager_OnGameStateChange(object sender, StateEnum stateChange)
    {
        if (stateChange == StateEnum.active)
        {
            pockewatchHand.eulerAngles = Vector3.zero;

            ToggleMovement(true);
            ToggleUI(true);
            OnShowUI?.Invoke();
        }
        else
        {
            ToggleMovement(false);
            ToggleUI(false);
        }
    }

    private void RewindManager_OnRewindToStart()
    {
        ToggleMovement(false);
        ToggleUI(false);
    }
}
