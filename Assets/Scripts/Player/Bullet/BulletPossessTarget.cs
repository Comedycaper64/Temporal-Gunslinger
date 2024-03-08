using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessTarget : MonoBehaviour, IHighlightable
{
    //private bool isHighlighted;

    private Bullet bullet;

    [SerializeField]
    private Transform highlight;
    private List<IHighlightable> highlightables = new List<IHighlightable>();
    private List<BulletPossessTarget> possessables = new List<BulletPossessTarget>();

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
    }

    public void PossessBullet()
    {
        bullet.ToggleBulletPossessed(true);
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
        return possessables;
    }

    public void AddHighlightable(IHighlightable highlightable)
    {
        highlightables.Add(highlightable);

        if (highlightable.GetType() == typeof(BulletPossessTarget))
        {
            possessables.Add(highlightable as BulletPossessTarget);
        }

        if (IsFocusing())
        {
            highlightable.ToggleHighlight(true);
        }
        //Make it remove if destroyed also
    }

    public void RemoveHighlightable(IHighlightable highlightable)
    {
        highlightables.Remove(highlightable);

        if (highlightable.GetType() == typeof(BulletPossessTarget))
        {
            possessables.Remove(highlightable as BulletPossessTarget);
        }

        highlightable.ToggleHighlight(false);
    }

    public void ToggleNearbyPossessableHighlight(bool toggle)
    {
        foreach (IHighlightable highlightable in highlightables)
        {
            highlightable.ToggleHighlight(toggle);
        }
    }

    public void ToggleHighlight(bool toggle)
    {
        highlight.gameObject.SetActive(toggle);
    }

    //public bool IsHighlighted() => isHighlighted;
}
