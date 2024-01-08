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
    private bool bIsFocusing;
    private bool bIsRewinding;
    public event Action OnShootAction;
    public event Action OnPossessAction;

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

    public Vector2 GetMouseMovement()
    {
        return mouseMovement;
    }

    public bool GetIsFocusing()
    {
        return bIsFocusing;
    }

    public bool GetIsRewinding()
    {
        return bIsRewinding;
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

    public void OnFocus(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bIsFocusing = true;
        }
        else if (context.canceled)
        {
            bIsFocusing = false;
        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bIsRewinding = true;
        }
        else if (context.canceled)
        {
            bIsRewinding = false;
        }
    }

    public void OnPossess(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        else
        {
            OnPossessAction?.Invoke();
        }
    }
}
