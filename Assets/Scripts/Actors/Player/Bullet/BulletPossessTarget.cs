using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPossessTarget : MonoBehaviour, IHighlightable
{
    public static bool highlightActive;

    [SerializeField]
    private bool possessableWhileInactive;

    private Bullet bullet;

    [SerializeField]
    private Transform highlight;
    public static EventHandler<BulletPossessTarget> OnEmergencyRepossess;
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

        bullet.OnShuntOut += EmergencyRepossess;

        if (possessableWhileInactive)
        {
            highlightables.Add(this);
            possessables.Add(this);
        }
        else
        {
            bullet.OnActiveToggled += ToggleBulletActive;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;

        bullet.OnShuntOut -= EmergencyRepossess;

        if (possessableWhileInactive)
        {
            highlightables.Remove(this);
            possessables.Remove(this);
        }
        else
        {
            bullet.OnActiveToggled -= ToggleBulletActive;
        }
    }

    private void EmergencyRepossess()
    {
        OnEmergencyRepossess?.Invoke(this, GetPossessables()[0]);
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

    public void ToggleLockOn(bool toggle) => bullet.ToggleLockOn(toggle);

    public void LockOnBullet() => bullet.LockOnBullet();

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

    public void ToggleNearbyPossessableHighlight(bool toggle)
    {
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

    public void ToggleBulletActive(object sender, bool toggle)
    {
        if (toggle)
        {
            highlightables.Add(this);
            possessables.Add(this);
        }
        else
        {
            highlightables.Remove(this);
            possessables.Remove(this);
        }
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
