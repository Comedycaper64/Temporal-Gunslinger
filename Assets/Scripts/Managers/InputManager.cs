using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public static InputManager Instance { get; private set; }
    private Controls controls;
    private Vector2 mouseMovement;
    private Vector2 mousePosition;
    private Vector2 freeCamMovement;
    private bool bIsFocusing;
    private bool bIsRewinding;
    private bool bIsTurbo;
    private bool bIsResetting;
    public event Action OnShootAction;
    public event Action OnShootReleaseAction;
    public event Action OnFocusAction;
    public event Action OnFocusReleaseAction;
    public event Action OnRewindAction;

    public event Action OnPossessAction;
    public event Action OnConquestAction;
    public event Action OnFamineAction;
    public event Action OnPauseAction;
    public event Action OnFreeCamAction;
    public event Action OnFreeCamPossessAction;
    public event EventHandler<int> OnChooseDialogueAction;

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

    public Vector2 GetFreeCamMovement()
    {
        return freeCamMovement;
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

        if (context.performed)
        {
            OnFocusAction?.Invoke();
        }
        else if (context.canceled)
        {
            OnFocusReleaseAction?.Invoke();
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
            OnRewindAction?.Invoke();
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

    public void OnFamineAbiltity(InputAction.CallbackContext context)
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
            OnFamineAction?.Invoke();
        }
    }

    public void OnFreeCam(InputAction.CallbackContext context)
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
            OnFreeCamAction?.Invoke();
        }
    }

    public void OnFreeCamPossess(InputAction.CallbackContext context)
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
            OnFreeCamPossessAction?.Invoke();
        }
    }

    public void OnFreeCamMovement(InputAction.CallbackContext context)
    {
        if (PauseMenuUI.pauseActive)
        {
            freeCamMovement = Vector2.zero;
            return;
        }

        freeCamMovement = context.ReadValue<Vector2>();
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

    public void OnSelectDialogue(InputAction.CallbackContext context)
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
            Vector2 choice = context.ReadValue<Vector2>();

            int dialogueChoice = 1;

            if (choice.y < 0f)
            {
                dialogueChoice = 2;
            }
            else if (choice.x < 0f)
            {
                dialogueChoice = 3;
            }
            else if (choice.x > 0f)
            {
                dialogueChoice = 4;
            }

            OnChooseDialogueAction?.Invoke(this, dialogueChoice);
        }
    }
}
