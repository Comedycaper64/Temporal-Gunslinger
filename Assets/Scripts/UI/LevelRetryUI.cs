using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRetryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject retryUI;

    private void Start()
    {
        GameManager.Instance.OnLevelLost += GameManager_OnLevelLost;
        retryUI.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelLost -= GameManager_OnLevelLost;
    }

    private void GameManager_OnLevelLost(object sender, bool e)
    {
        retryUI.SetActive(e);
    }
}
