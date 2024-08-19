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
    public event Action OnShootReleaseAction;
    public event Action OnFocusAction;
    public event Action OnPossessAction;
    public event Action OnConquestAction;
    public event Action OnPauseAction;

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

        //PauseMenuUI.OnPauseToggled += ToggleLookInput;
    }

    // private void OnDisable()
    // {
    //     PauseMenuUI.OnPauseToggled -= ToggleLookInput;
    // }

    // private void ToggleLookInput(object sender, bool toggle)
    // {
    //     if (toggle)
    //     {
    //         controls.Player.Look.Disable();
    //     }
    //     else
    //     {
    //         controls.Player.Look.Enable();
    //     }
    // }

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
        mouseMovement = context.ReadValue<Vector2>();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

        if (context.performed)
        {
            OnShootAction?.Invoke();
        }
        else if (context.canceled)
        {
            OnShootReleaseAction?.Invoke();
        }
    }

    public void OnFocus(InputAction.CallbackContext context)
    {
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

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
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

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
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }
        else
        {
            OnPossessAction?.Invoke();
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

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
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

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
        if (PauseMenuUI.pauseActive)
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }
        else
        {
            OnConquestAction?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        else
        {
            OnPauseAction?.Invoke();
        }
    }
}
