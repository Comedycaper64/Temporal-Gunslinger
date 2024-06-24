using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessTarget : MonoBehaviour, IHighlightable
{
    public static bool highlightActive;

    private Bullet bullet;

    [SerializeField]
    private Transform highlight;
    public static HashSet<IHighlightable> highlightables = new HashSet<IHighlightable>();
    private static HashSet<BulletPossessTarget> possessables = new HashSet<BulletPossessTarget>();

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
        highlightActive = false;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
        highlightables.Add(this);
        possessables.Add(this);
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
        highlightables.Remove(this);
        possessables.Remove(this);
    }

    public void PossessBullet(bool isFocusing)
    {
        bullet.ToggleBulletPossessed(true);
        SetIsFocusing(isFocusing);
    }

    public void UnpossessBullet()
    {
        bullet.ToggleBulletPossessed(false);
        SetIsFocusing(false);
    }

    public void RedirectBullet() => bullet.RedirectBullet();

    public void SetIsFocusing(bool isFocusing)
    {
        bullet.SetIsFocusing(isFocusing);
        ToggleNearbyPossessableHighlight(isFocusing);
    }

    public bool IsFocusing() => bullet.IsFocusing();

    public List<BulletPossessTarget> GetPossessables()
    {
        List<BulletPossessTarget> targets = new List<BulletPossessTarget>(possessables);
        targets.Remove(this);

        return targets;
    }

    // public void AddHighlightable(IHighlightable highlightable)
    // {
    //     highlightables.Add(highlightable);

    //     if (highlightable.GetType() == typeof(BulletPossessTarget))
    //     {
    //         possessables.Add(highlightable as BulletPossessTarget);
    //     }

    //     if (IsFocusing())
    //     {
    //         highlightable.ToggleHighlight(true);
    //     }
    //     //Make it remove if destroyed also
    // }

    // public void RemoveHighlightable(IHighlightable highlightable)
    // {
    //     highlightables.Remove(highlightable);

    //     if (highlightable.GetType() == typeof(BulletPossessTarget))
    //     {
    //         possessables.Remove(highlightable as BulletPossessTarget);
    //     }

    //     highlightable.ToggleHighlight(false);
    // }

    public void ToggleNearbyPossessableHighlight(bool toggle)
    {
        // for (int i = 0; i < highlightables.Count; i++)
        // {
        //     if ((object)highlightables[i] == this)
        //     {
        //         continue;
        //     }

        //     highlightables[i].ToggleHighlight(toggle);
        // }

        foreach (IHighlightable highlightable in highlightables)
        {
            //Debug.Log("Highlightable in list: " + highlightable);

            if ((object)highlightable == this)
            {
                continue;
            }

            highlightable.ToggleHighlight(toggle);
            highlightActive = toggle;
        }
    }

    public void ToggleHighlight(bool toggle)
    {
        highlight.gameObject.SetActive(toggle);
    }

    private void GameManager_OnGameStateChange(object sender, StateEnum e)
    {
        if (e == StateEnum.inactive)
        {
            UnpossessBullet();
        }
    }

    //public bool IsHighlighted() => isHighlighted;
}
