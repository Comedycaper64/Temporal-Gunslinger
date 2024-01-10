using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private State currentState;
    private float mouseSensitivity = 15f;
    private float xRotation = 0f;
    private bool bMovementEnabled = false;
    private bool bIsFocusing = false;
    private InputManager input;
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

    void Start()
    {
        input = InputManager.Instance;
        input.OnShootAction += InputManager_OnShootAction;
        input.OnPossessAction += InputManager_OnPossessAction;
    }

    private void OnDisable()
    {
        input.OnShootAction -= InputManager_OnShootAction;
        input.OnPossessAction -= InputManager_OnPossessAction;
    }

    void Update()
    {
        if (input.GetIsFocusing() != bIsFocusing)
        {
            bIsFocusing = !bIsFocusing;
            IsFocusingChanged(bIsFocusing);
        }

        if (!bMovementEnabled)
        {
            return;
        }

        RotatePlayer();
    }

    //Rotates player view and player model based on mouse movement
    private void RotatePlayer()
    {
        Vector2 mouseMovement = input.GetMouseMovement() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseMovement.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseMovement.x);
    }

    public void TogglePlayerController(bool toggle)
    {
        bMovementEnabled = toggle;

        if (bMovementEnabled)
        {
            playerGun.ToggleAimGun(false);
        }
    }

    private void InputManager_OnShootAction()
    {
        if (!bIsFocusing)
        {
            return;
        }

        if (!bMovementEnabled)
        {
            bulletPossessor.RedirectBullet();
        }
        else
        {
            GameManager.Instance.LevelStart();
            bulletPossessor.PossessBullet(initialBullet);
        }
    }

    private void InputManager_OnPossessAction()
    {
        bulletPossessor.TryPossess();
    }

    private void IsFocusingChanged(bool isFocusing)
    {
        if (!bMovementEnabled)
        {
            bulletPossessor.SetIsFocusing(isFocusing);
        }
        else
        {
            playerGun.ToggleAimGun(isFocusing);
        }
    }
}
