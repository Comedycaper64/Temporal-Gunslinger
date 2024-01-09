using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRetryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject retryUI;

    private void Awake()
    {
        PlayerDeadState.OnPlayerDied += PlayerDeadState_OnPlayerDied;
        retryUI.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerDeadState.OnPlayerDied -= PlayerDeadState_OnPlayerDied;
    }

    private void PlayerDeadState_OnPlayerDied(object sender, bool e)
    {
        retryUI.SetActive(e);
    }
}
