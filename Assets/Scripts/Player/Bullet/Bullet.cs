using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private bool bBulletActive;
    private BulletMovement bulletMovement;
    private BulletCameraController bulletCameraController;
    private RedirectManager redirectManager;
    private FocusManager focusManager;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletCameraController = GetComponent<BulletCameraController>();
        redirectManager = GetComponent<RedirectManager>();
        focusManager = GetComponent<FocusManager>();
    }

    private void Start()
    {
        InputManager.Instance.OnShootAction += RedirectBullet;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnShootAction -= RedirectBullet;
    }

    private void RedirectBullet()
    {
        if (bBulletActive && redirectManager.TryRedirect())
        {
            Redirect.BulletRedirected(transform.position, bulletMovement.GetRotation(), this);
            bulletMovement.RedirectBullet(bulletCameraController.GetCameraForward());
        }
        else
        {
            //Sound effect or other indicator
        }
    }

    public void UndoRedirect(Vector3 position, Quaternion rotation)
    {
        bulletMovement.RedirectBullet(rotation);
        transform.position = position;
        redirectManager.IncrementRedirects();
    }

    public void ToggleBulletActive(bool toggle)
    {
        bulletCameraController.ToggleCamera(toggle);
        bulletMovement.ToggleBulletMovement(toggle);
        focusManager.ToggleCanFocus(toggle);
        bBulletActive = toggle;
    }

    public bool IsBulletActive()
    {
        return bBulletActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(this);
        }
    }
}
