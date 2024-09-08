using System;
using UnityEngine;

public class RedirectManager : MonoBehaviour
{
    public static RedirectManager Instance { get; private set; }

    [SerializeField]
    private int levelRedirects;

    private int redirects = 0;

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

    public void SetRedirects(int newRedirects)
    {
        redirects = newRedirects;
        OnRedirectsChanged?.Invoke(this, redirects);
    }

    public void IncrementRedirects()
    {
        SetRedirects(++redirects);
    }

    public void ToggleRedirectUI(bool toggle)
    {
        OnRedirectUIActive?.Invoke(this, toggle);
    }

    public void ResetLevel()
    {
        SetRedirects(levelRedirects);
    }
}
