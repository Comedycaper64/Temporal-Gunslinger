using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketwatchUI : RewindableMovement
{
    private bool uiActive = false;
    private bool uiChange = false;
    private float fadeSpeed = 5f;
    private float currentPocketwatchTime = 0f;
    private float deathTime = 0f;

    [SerializeField]
    private Transform pockewatchHand;

    [SerializeField]
    private Transform markerPlacerHand;

    [SerializeField]
    private Transform markerPlacerTip;

    [SerializeField]
    private Transform deathMarker;

    [SerializeField]
    private CanvasGroup pocketwatchUI;

    private void Start()
    {
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
        RewindManager.OnRewindToStart += RewindManager_OnRewindToStart;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
        RewindManager.OnRewindToStart -= RewindManager_OnRewindToStart;
    }

    private void Update()
    {
        if (uiChange)
        {
            FadeUI();
        }

        if (uiActive)
        {
            pockewatchHand.eulerAngles =
                pockewatchHand.eulerAngles + new Vector3(0, 0, -GetSpeed() * Time.deltaTime);
            currentPocketwatchTime += GetSpeed() * Time.deltaTime;
        }
    }

    private void FadeUI()
    {
        int direction;
        if (uiActive)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        pocketwatchUI.alpha += fadeSpeed * Time.deltaTime * direction;

        if (uiActive)
        {
            if (pocketwatchUI.alpha >= 1f)
            {
                uiChange = false;
            }
        }
        else
        {
            if (pocketwatchUI.alpha <= 0f)
            {
                uiChange = false;
            }
        }
    }

    private void ToggleUI(bool toggle)
    {
        uiActive = toggle;
        uiChange = true;
        currentPocketwatchTime = 0f;
    }

    public void SetDeathTime(float newDeathTime)
    {
        Debug.Log(newDeathTime);

        deathTime = newDeathTime;

        if (deathTime < 0f)
        {
            deathMarker.gameObject.SetActive(false);
        }
        else
        {
            deathMarker.gameObject.SetActive(true);
            markerPlacerHand.eulerAngles = new Vector3(0, 0, -GetStartSpeed() * deathTime);
            //Debug.Log(GetUnscaledSpeed() * deathTime);
            deathMarker.position = markerPlacerTip.position;
        }
    }

    private void GameManager_OnGameStateChange(object sender, StateEnum stateChange)
    {
        if (stateChange == StateEnum.active)
        {
            pockewatchHand.eulerAngles = Vector3.zero;

            ToggleMovement(true);
            ToggleUI(true);
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
