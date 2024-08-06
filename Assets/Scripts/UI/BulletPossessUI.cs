using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessUI : MonoBehaviour
{
    private Transform possessTarget;

    [SerializeField]
    private Transform possessUI;

    private Vector3 uiOffset = new Vector3(0, 0.1f, 0);

    private void OnEnable()
    {
        BulletPossessor.OnNewCentralPossessable += SetUITarget;
        possessUI.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        BulletPossessor.OnNewCentralPossessable -= SetUITarget;
    }

    private void Update()
    {
        if (possessTarget)
        {
            Vector3 viewPos = Camera.main.WorldToScreenPoint(possessTarget.position + uiOffset);
            possessUI.position = viewPos;
        }
    }

    private void SetUITarget(object sender, BulletPossessTarget e)
    {
        if (!e)
        {
            possessUI.gameObject.SetActive(false);
        }
        else
        {
            possessUI.gameObject.SetActive(true);
            possessTarget = e.transform;
        }
    }
}
