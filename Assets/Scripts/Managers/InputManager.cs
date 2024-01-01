using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public static InputManager Instance { get; private set; }
    private Controls controls;
    private Vector2 mouseMovement;
    public event Action OnShootAction;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        mouseMovement = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        else
        {
            OnShootAction?.Invoke();
        }
    }

    public Vector2 GetMouseMovement()
    {
        return mouseMovement;
    }
}
