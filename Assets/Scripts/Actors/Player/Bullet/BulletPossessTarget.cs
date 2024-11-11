using System;
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

    // [SerializeField]
    // private Renderer mainBulletRenderer;
    public static EventHandler<BulletPossessTarget> OnEmergencyRepossess;
    public static HashSet<IHighlightable> highlightables = new HashSet<IHighlightable>();
    private static HashSet<BulletPossessTarget> possessables = new HashSet<BulletPossessTarget>();

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
        highlightActive = false;
        // possessables = new HashSet<BulletPossessTarget>();
        // highlightables = new HashSet<IHighlightable>();
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
            highlightables?.Remove(this);
            possessables?.Remove(this);
        }
        else
        {
            bullet.OnActiveToggled -= ToggleBulletActive;
            ToggleBulletActive(null, false);
        }
    }

    private void EmergencyRepossess()
    {
        OnEmergencyRepossess?.Invoke(this, GetPossessables()[0]);
    }

    public void PossessBullet(bool isFocusing, Vector2 newCameraAxis)
    {
        bullet.ToggleBulletPossessed(true, newCameraAxis);
        SetIsFocusing(isFocusing);
    }

    public void UnpossessBullet()
    {
        bullet.ToggleBulletPossessed(false, Vector2.zero);
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

    public Vector2 GetCameraAxisValues() => bullet.GetCameraAxisValues();

    public Transform GetCameraTransform() => bullet.GetCameraTransform();

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
            if ((object)highlightable == this)
            {
                highlightable.ToggleHighlight(false);
                continue;
            }

            highlightable.ToggleHighlight(toggle);
            highlightActive = toggle;
        }
    }

    public void ToggleHighlight(bool toggle)
    {
        if (!highlight)
        {
            return;
        }

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
            highlightables?.Remove(this);
            possessables?.Remove(this);
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
