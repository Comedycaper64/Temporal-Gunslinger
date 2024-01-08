using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessTarget : MonoBehaviour
{
    private Bullet bullet;

    // [SerializeField]
    // private BulletTriggerTracker bulletTriggerTracker;
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
    }

    public void RedirectBullet() => bullet.RedirectBullet();

    public void SetIsFocusing(bool isFocusing) => bullet.SetIsFocusing(isFocusing);

    public bool IsFocusing() => bullet.IsFocusing();

    public List<BulletPossessTarget> GetPossessables()
    {
        return possessables;
    }

    public void AddPossessable(BulletPossessTarget possessable)
    {
        possessables.Add(possessable);
        //Make it remove if destroyed also
    }

    public void RemovePossessable(BulletPossessTarget possessable)
    {
        possessables.Remove(possessable);
        //Stop highlighter
    }
}
