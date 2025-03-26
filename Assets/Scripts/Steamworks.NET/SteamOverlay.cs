using System;
using Steamworks;
using UnityEngine;

public class SteamOverlay : MonoBehaviour
{
    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    public static Action OnOverlayEnable;

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(
                OnGameOverlayActivated
            );
        }
    }

    private void OnDisable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated.Dispose();
        }
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
            OnOverlayEnable?.Invoke();
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }
}
