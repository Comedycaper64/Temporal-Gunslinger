using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public enum CutInType
{
    Conquest
}

public class AbilityCutInUI : MonoBehaviour
{
    private float cutInFadeInTime = 1f;
    private float cutInDisableTime = 2f;

    [SerializeField]
    private GameObject cutInCamera;

    [SerializeField]
    private GameObject[] cutIns;

    private CanvasGroupFader canvasGroupFader;
    private Coroutine cutInCoroutine;

    private void Awake()
    {
        canvasGroupFader = GetComponent<CanvasGroupFader>();
        canvasGroupFader.SetCanvasGroupAlpha(0f);
        DisableCutIns();
    }

    private void OnEnable()
    {
        PlayerConquestAbility.OnConquestAbility += TriggerCutIn;
    }

    private void OnDisable()
    {
        PlayerConquestAbility.OnConquestAbility -= TriggerCutIn;
    }

    private void TriggerCutIn(object sender, CutInType cutIn)
    {
        GameObject currentCutIn = cutIns[(int)cutIn];

        cutInCamera.SetActive(true);
        currentCutIn.SetActive(true);
        canvasGroupFader.ToggleFade(true, 0.5f, MMTween.MMTweenCurve.EaseOutCubic);

        if (cutInCoroutine != null)
        {
            StopCoroutine(cutInCoroutine);
        }

        cutInCoroutine = StartCoroutine(CutInWindDown());
    }

    private void DisableCutIns()
    {
        cutInCamera.SetActive(false);
        foreach (GameObject cutin in cutIns)
        {
            cutin.SetActive(false);
        }
    }

    private IEnumerator CutInWindDown()
    {
        yield return new WaitForSecondsRealtime(cutInFadeInTime);

        canvasGroupFader.ToggleFade(false, 0f, MMTween.MMTweenCurve.EaseOutCubic);

        yield return new WaitForSecondsRealtime(cutInDisableTime);

        DisableCutIns();
    }
}
