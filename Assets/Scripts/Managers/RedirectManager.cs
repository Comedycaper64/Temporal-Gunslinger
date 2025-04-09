using System;
using UnityEngine;

public class RedirectManager : MonoBehaviour, IReactable
{
    public static RedirectManager Instance { get; private set; }

    [SerializeField]
    private int levelRedirects;
    private static int redirectIndex;

    private int redirects = 0;

    [SerializeField]
    private TrickshotCoin[] localRedirectCoins;
    private static TrickshotCoin[] redirectCoins;

    public static Action OnRedirectFailed;
    public static event EventHandler<int> OnRedirectsChanged;
    public static event EventHandler<bool> OnRedirectUIActive;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one RedirectManager! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;
        redirectIndex = 0;
        redirectCoins = localRedirectCoins;
    }

    //Debug, redirects should be set in level/game manager
    private void Start()
    {
        SetRedirects(levelRedirects);
    }

    public bool TryRedirect()
    {
        if (redirects > 0)
        {
            redirects--;
            OnRedirectsChanged?.Invoke(this, redirects);
            return true;
        }
        else
        {
            OnRedirectFailed?.Invoke();
            return false;
        }
    }

    public bool CanRedirect()
    {
        if (redirects <= 0)
        {
            OnRedirectFailed?.Invoke();
        }

        return redirects > 0;
    }

    public bool CanBoost()
    {
        return redirects > 1;
    }

    public int GetRemainingRedirects()
    {
        return redirects;
    }

    public void SetRedirects(int newRedirects)
    {
        redirects = newRedirects;
        OnRedirectsChanged?.Invoke(this, redirects);
    }

    public void IncrementRedirects()
    {
        SetRedirects(++redirects);
    }

    public void DecrementRedirects()
    {
        SetRedirects(--redirects);
    }

    public void ToggleRedirectUI(bool toggle)
    {
        OnRedirectUIActive?.Invoke(this, toggle);
    }

    public void ResetLevel()
    {
        SetRedirects(levelRedirects);
    }

    public static void SpawnRedirectCoin(Vector3 spawnPosition)
    {
        redirectCoins[redirectIndex].gameObject.SetActive(true);
        redirectCoins[redirectIndex].SetCoin(spawnPosition);

        redirectIndex++;

        if (redirectIndex >= redirectCoins.Length)
        {
            redirectIndex = 0;
        }

        StartReaction.ReactionStarted(Instance);
    }

    public void UndoReaction()
    {
        redirectIndex--;

        if (redirectIndex < 0)
        {
            redirectIndex = redirectCoins.Length - 1;
        }
        redirectCoins[redirectIndex].gameObject.SetActive(false);
    }
}
