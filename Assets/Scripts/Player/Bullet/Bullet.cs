using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player's bullet
public class Bullet : MonoBehaviour
{
    private BulletMovement bulletMovement;
    private BulletCameraController bulletCameraController;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletCameraController = GetComponent<BulletCameraController>();
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
        if (RedirectManager.Instance.TryRedirect())
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
        RedirectManager.Instance.IncrementRedirects();
    }
}
