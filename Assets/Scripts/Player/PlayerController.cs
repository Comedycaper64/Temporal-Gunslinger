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
    private InputManager input;

    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private PlayerGun playerGun;

    void Start()
    {
        input = InputManager.Instance;
        input.OnShootAction += FireGun;
    }

    private void OnDisable()
    {
        input.OnShootAction -= FireGun;
    }

    void Update()
    {
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
    }

    private void FireGun()
    {
        if (!bMovementEnabled)
        {
            return;
        }
        //Gun fired signal to Game Manager
        GameManager.Instance.LevelStart();
        playerGun.ShootBullet();
    }
}
