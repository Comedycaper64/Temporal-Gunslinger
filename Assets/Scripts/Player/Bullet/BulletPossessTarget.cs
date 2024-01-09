using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessTarget : MonoBehaviour
{
    //private bool isHighlighted;

    private Bullet bullet;

    [SerializeField]
    private GameObject targetHighlight;
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

    public void AddPossessable(BulletPossessTarget possessable)
    {
        possessables.Add(possessable);
        if (IsFocusing())
        {
            possessable.ToggleTargetHighlight(true);
        }
        //Make it remove if destroyed also
    }

    public void RemovePossessable(BulletPossessTarget possessable)
    {
        possessables.Remove(possessable);
        possessable.ToggleTargetHighlight(false);
    }

    public void ToggleTargetHighlight(bool toggle)
    {
        targetHighlight.SetActive(toggle);
        //isHighlighted = toggle;
    }

    public void ToggleNearbyPossessableHighlight(bool toggle)
    {
        foreach (BulletPossessTarget target in possessables)
        {
            target.ToggleTargetHighlight(toggle);
        }
    }

    //public bool IsHighlighted() => isHighlighted;
}
