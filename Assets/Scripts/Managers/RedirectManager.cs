using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectManager : MonoBehaviour
{
    public static RedirectManager Instance { get; private set; }

    private int redirects = 0;

    public event EventHandler<int> OnRedirectsChanged;

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
        SetRedirects(3);
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
            return false;
        }
    }

    public void SetRedirects(int newRedirects)
    {
        redirects = newRedirects;
        OnRedirectsChanged?.Invoke(this, redirects);
    }
}
