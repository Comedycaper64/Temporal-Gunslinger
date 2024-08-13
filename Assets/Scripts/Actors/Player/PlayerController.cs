using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float REST_MOUSE_SENSITIVITY = 10f;
    private const float AIM_MOUSE_SENSITIVITY = 5f;

    [SerializeField]
    private float mouseSensitivity = REST_MOUSE_SENSITIVITY;
    private float xRotation = 0f;
    private bool bBulletFired = false;
    private bool bIsPlayerActive = false;
    private bool bIsFocusing = false;

    //Tutorial bools
    private bool bCanRotate = true;
    private bool bCanRedirect = true;
    private bool bCanPossess = true;
    private bool bCanFocus = true;

    private BulletPossessor bulletPossessor;

    [SerializeField]
    private BulletPossessTarget initialBullet;

    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private PlayerGun playerGun;

    public static EventHandler<int> OnPlayerStateChanged;

    [SerializeField]
    private AudioClip readyGunSFX;

    private void Awake()
    {
        bulletPossessor = GetComponent<BulletPossessor>();
    }

    private void Start()
    {
        InputManager.Instance.OnFocusAction += InputManager_OnFocus;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnFocusAction -= InputManager_OnFocus;
    }

    void Update()
    {
        if (!bIsPlayerActive && !bBulletFired)
        {
            return;
        }

        // if (InputManager.Instance.GetIsFocusing() != bIsFocusing)
        // {
        //     bIsFocusing = !bIsFocusing;
        //     IsFocusingChanged(bIsFocusing);
        // }

        if (bBulletFired)
        {
            return;
        }

        RotatePlayer();
    }

    //Rotates player view and player model based on mouse movement
    private void RotatePlayer()
    {
        if (!bCanRotate)
        {
            return;
        }

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
        bIsFocusing = false;

        if (bIsPlayerActive)
        {
            InputManager.Instance.OnShootAction += InputManager_OnShootAction;
            OnPlayerStateChanged?.Invoke(this, 1);
            playerGun.ResetBullet();
            playerGun.SetGunStandbyPosition();
            bulletPossessor.SetIsFocusing(false);
            bulletPossessor.UndoPossess(null);
            AudioManager.PlaySFX(readyGunSFX, 0.5f, 0, transform.position);
            if (bCanRedirect)
            {
                RedirectManager.Instance.ToggleRedirectUI(true);
            }
        }
        else
        {
            InputManager.Instance.OnShootAction -= InputManager_OnShootAction;
            OnPlayerStateChanged?.Invoke(this, 0);
            RedirectManager.Instance.ToggleRedirectUI(false);
        }
    }

    public void ToggleBulletFired(bool toggle)
    {
        bBulletFired = toggle;

        if (bBulletFired)
        {
            InputManager.Instance.OnShootAction += InputManager_OnRedirectAction;
            InputManager.Instance.OnPossessAction += InputManager_OnPossessAction;
            // InputManager.Instance.OnPossessNextAction += InputManager_OnPossessNextAction;
            // InputManager.Instance.OnPossessPreviousAction += InputManager_OnPossessPreviousAction;
            if (bCanRedirect)
            {
                RedirectManager.Instance.ToggleRedirectUI(true);
            }
        }
        else
        {
            InputManager.Instance.OnShootAction -= InputManager_OnRedirectAction;
            InputManager.Instance.OnPossessAction -= InputManager_OnPossessAction;
            // InputManager.Instance.OnPossessNextAction -= InputManager_OnPossessNextAction;
            // InputManager.Instance.OnPossessPreviousAction -= InputManager_OnPossessPreviousAction;
            OnPlayerStateChanged?.Invoke(this, 0);
            RedirectManager.Instance.ToggleRedirectUI(false);
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

        if (!bCanFocus)
        {
            return;
        }

        OnPlayerStateChanged?.Invoke(this, 3);
    }

    private void InputManager_OnFocus()
    {
        if (!bIsPlayerActive && !bBulletFired)
        {
            return;
        }

        bIsFocusing = !bIsFocusing;

        if (bIsFocusing)
        {
            mouseSensitivity = AIM_MOUSE_SENSITIVITY;
        }
        else
        {
            mouseSensitivity = REST_MOUSE_SENSITIVITY;
        }

        IsFocusingChanged(bIsFocusing);
    }

    private void InputManager_OnRedirectAction()
    {
        if (!bIsFocusing || !bCanRedirect)
        {
            return;
        }

        bulletPossessor.RedirectBullet();
    }

    private void InputManager_OnPossessAction()
    {
        if (!GameManager.Instance.IsLevelActive() || !bCanPossess)
        {
            return;
        }
        bulletPossessor.TryPossess();
    }

    // private void InputManager_OnPossessPreviousAction()
    // {
    //     if (!GameManager.Instance.IsLevelActive() || !bCanPossess)
    //     {
    //         return;
    //     }
    //     bulletPossessor.TryPossessPrevious();
    // }

    // private void InputManager_OnPossessNextAction()
    // {
    //     if (!GameManager.Instance.IsLevelActive() || !bCanPossess)
    //     {
    //         return;
    //     }
    //     bulletPossessor.TryPossessNext();
    // }

    private void IsFocusingChanged(bool isFocusing)
    {
        if (bBulletFired)
        {
            if (!bCanFocus)
            {
                return;
            }

            bulletPossessor.SetIsFocusing(isFocusing);
            if (isFocusing)
            {
                OnPlayerStateChanged?.Invoke(this, 4);
            }
            else
            {
                OnPlayerStateChanged?.Invoke(this, 3);
            }
        }
        else
        {
            playerGun.ToggleAimGun(isFocusing);
            if (isFocusing)
            {
                OnPlayerStateChanged?.Invoke(this, 2);
            }
            else
            {
                OnPlayerStateChanged?.Invoke(this, 1);
            }
        }
    }

    public void ResetPlayerRotation()
    {
        playerCamera.localRotation = Quaternion.identity;
        playerBody.rotation = Quaternion.identity;
    }

    public void ToggleTutorialStartMode()
    {
        ToggleCanRotate(false);
        ToggleCanPossess(false);
        ToggleCanRedirect(false);
        ToggleCanFocus(false);
    }

    public void ToggleCanRotate(bool toggle)
    {
        bCanRotate = toggle;
    }

    public void ToggleCanPossess(bool toggle)
    {
        bCanPossess = toggle;
    }

    public void ToggleCanRedirect(bool toggle)
    {
        bCanRedirect = toggle;
    }

    public void ToggleCanFocus(bool toggle)
    {
        bCanFocus = toggle;
    }
}
