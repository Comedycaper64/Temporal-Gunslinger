using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InspectUI : MonoBehaviour
{
    private bool bCrosshairActive = false;
    private Transform crosshairTarget;

    [SerializeField]
    private int uiLayer;

    [SerializeField]
    private CanvasGroupFader inspectControlUI;

    [SerializeField]
    private CanvasGroupFader inspectUI;

    [SerializeField]
    private CanvasGroupFader inspectCrosshairUI;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI revenantText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Transform cameraObjectPlacement;

    private InspectTarget spawnedObject;

    private void Awake()
    {
        InspectManager.OnInspect += ToggleUI;
        InspectManager.OnCanInspect += ToggleControlUI;
    }

    private void OnDisable()
    {
        InspectManager.OnInspect -= ToggleUI;
        InspectManager.OnCanInspect -= ToggleControlUI;
    }

    private void LateUpdate()
    {
        if (bCrosshairActive)
        {
            inspectCrosshairUI.transform.position = GetCrosshairLocation(crosshairTarget);
        }

        if (spawnedObject)
        {
            spawnedObject.transform.Rotate(0f, 50f * Time.deltaTime, 0f);
        }
    }

    private void ToggleUI(object sender, InspectTarget target)
    {
        if (target == null)
        {
            inspectUI.ToggleFade(false);
            if (spawnedObject)
            {
                spawnedObject.ResetTarget();
                spawnedObject = null;
            }
            return;
        }

        spawnedObject = target;

        titleText.text = target.GetTargetName();
        revenantText.text = target.GetTargetThoughts();
        descriptionText.text = target.GetTargetDescription();

        spawnedObject.gameObject.layer = uiLayer;

        foreach (GameObject subObject in spawnedObject.GetTargetSubObjects())
        {
            subObject.layer = uiLayer;
        }

        spawnedObject.transform.position = cameraObjectPlacement.position;
        spawnedObject.transform.rotation = cameraObjectPlacement.rotation;

        float viewScale = target.GetTargetViewScale();
        spawnedObject.transform.localScale = new Vector3(viewScale, viewScale, viewScale);

        inspectUI.ToggleFade(true);
    }

    private Vector3 GetCrosshairLocation(Transform inspectTarget)
    {
        return Camera.main.WorldToScreenPoint(inspectTarget.position);
    }

    private void ToggleControlUI(object sender, InspectTarget target)
    {
        if (target != null)
        {
            bCrosshairActive = true;
            crosshairTarget = target.transform;
            inspectControlUI.ToggleFade(true);
            inspectCrosshairUI.ToggleFade(true);
        }
        else
        {
            bCrosshairActive = false;
            inspectControlUI.ToggleFade(false);
            inspectCrosshairUI.ToggleFade(false);
        }
    }
}
