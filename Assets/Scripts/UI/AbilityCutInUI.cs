using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public enum CutInType
{
    Conquest,
    Famine
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
        PlayerFamineAbility.OnFamineAbility += TriggerCutIn;
    }

    private void OnDisable()
    {
        PlayerConquestAbility.OnConquestAbility -= TriggerCutIn;
        PlayerFamineAbility.OnFamineAbility -= TriggerCutIn;
    }

    private void TriggerCutIn(object sender, CutInType cutIn)
    {
        GameObject currentCutIn = cutIns[(int)cutIn];
        if (cutInCoroutine != null)
        {
            StopCoroutine(cutInCoroutine);
            DisableCutIns();
        }
        cutInCamera.SetActive(true);
        currentCutIn.SetActive(true);
        canvasGroupFader.ToggleFade(true, 0.5f, MMTween.MMTweenCurve.EaseOutCubic);

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
