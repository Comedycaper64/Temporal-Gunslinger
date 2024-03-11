using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private State currentState;
    private float mouseSensitivity = 15f;
    private float xRotation = 0f;
    private bool bBulletFired = false;
    private bool bIsPlayerActive = false;
    private bool bIsFocusing = false;
    private BulletPossessor bulletPossessor;

    [SerializeField]
    private BulletPossessTarget initialBullet;

    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private PlayerGun playerGun;

    private void Awake()
    {
        bulletPossessor = GetComponent<BulletPossessor>();
    }

    void Update()
    {
        if (!bIsPlayerActive && !bBulletFired)
        {
            return;
        }

        if (InputManager.Instance.GetIsFocusing() != bIsFocusing)
        {
            bIsFocusing = !bIsFocusing;
            IsFocusingChanged(bIsFocusing);
        }

        if (bBulletFired)
        {
            return;
        }

        RotatePlayer();
    }

    //Rotates player view and player model based on mouse movement
    private void RotatePlayer()
    {
        Vector2 mouseMovement =
            InputManager.Instance.GetMouseMovement() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseMovement.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseMovement.x);
    }

    public void TogglePlayerController(bool toggle)
    {
        bIsPlayerActive = toggle;

        if (bIsPlayerActive)
        {
            InputManager.Instance.OnShootAction += InputManager_OnShootAction;
        }
        else
        {
            InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
        }
    }

    public void ToggleBulletFired(bool toggle)
    {
        bBulletFired = toggle;

        if (bBulletFired)
        {
            InputManager.Instance.OnShootAction += InputManager_OnRedirectAction;
            InputManager.Instance.OnPossessAction += InputManager_OnPossessAction;
        }
        else
        {
            InputManager.Instance.OnShootAction -= InputManager_OnRedirectAction;
            InputManager.Instance.OnPossessAction -= InputManager_OnPossessAction;
        }

        // if (bBulletFired)
        // {
        //     playerGun.ToggleAimGun(false);
        // }
    }

    // public void DisableGun()
    // {
    //     playerGun.DisableBullet();
    // }

    private void InputManager_OnShootAction()
    {
        if (!bIsFocusing)
        {
            return;
        }

        GameManager.Instance.LevelStart();
        playerGun.FireGun();
        bulletPossessor.PossessBullet(initialBullet);
    }

    private void InputManager_OnRedirectAction()
    {
        if (!bIsFocusing)
        {
            return;
        }

        bulletPossessor.RedirectBullet();
    }

    private void InputManager_OnPossessAction()
    {
        if (!GameManager.Instance.IsLevelActive())
        {
            return;
        }
        bulletPossessor.TryPossess();
    }

    private void IsFocusingChanged(bool isFocusing)
    {
        if (bBulletFired)
        {
            bulletPossessor.SetIsFocusing(isFocusing);
        }
        else
        {
            playerGun.ToggleAimGun(isFocusing);
        }
    }
}
