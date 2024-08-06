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
    private Vector2 mousePosition;
    private bool bIsFocusing;
    private bool bIsRewinding;
    private bool bIsTurbo;
    private bool bIsResetting;
    public event Action OnShootAction;
    public event Action OnFocusAction;
    public event Action OnPossessAction;

    // public event Action OnPossessNextAction;
    // public event Action OnPossessPreviousAction;
    public event Action OnConquestAction;

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

    public Vector2 GetMousePosition()
    {
        return mousePosition;
    }

    public bool GetIsFocusing()
    {
        return bIsFocusing;
    }

    public bool GetIsRewinding()
    {
        return bIsRewinding;
    }

    public bool GetIsTurbo()
    {
        return bIsTurbo;
    }

    public bool GetIsResetting()
    {
        return bIsResetting;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        mouseMovement = context.ReadValue<Vector2>();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
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
        // if (context.performed)
        // {
        //     bIsFocusing = true;
        // }
        // else if (context.canceled)
        // {
        //     bIsFocusing = false;
        // }

        if (!context.performed)
        {
            return;
        }
        else
        {
            OnFocusAction?.Invoke();
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

    // public void OnPossessNext(InputAction.CallbackContext context)
    // {
    //     if (!context.performed)
    //     {
    //         return;
    //     }
    //     else
    //     {
    //         OnPossessNextAction?.Invoke();
    //     }
    // }

    // public void OnPossessPrevious(InputAction.CallbackContext context)
    // {
    //     if (!context.performed)
    //     {
    //         return;
    //     }
    //     else
    //     {
    //         OnPossessPreviousAction?.Invoke();
    //     }
    // }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bIsResetting = true;
        }
        else if (context.canceled)
        {
            bIsResetting = false;
        }
    }

    public void OnTurbo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bIsTurbo = true;
        }
        else if (context.canceled)
        {
            bIsTurbo = false;
        }
    }

    public void OnConquestAbility(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        else
        {
            OnConquestAction?.Invoke();
        }
    }
}
