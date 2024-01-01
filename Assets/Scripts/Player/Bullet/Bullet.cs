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
        bulletMovement.RedirectBullet(bulletCameraController.GetCameraRotation());
    }
}
