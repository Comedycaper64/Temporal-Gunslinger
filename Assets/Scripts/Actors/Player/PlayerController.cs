using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float REST_MOUSE_SENSITIVITY = 10f;
    private const float AIM_MOUSE_SENSITIVITY = 5f;
    private float PLAYER_MOUSE_SENSITIVITY = 1f;

    [SerializeField]
    private float mouseSensitivity = REST_MOUSE_SENSITIVITY;
    private float xRotation = 0f;
    private bool bBulletFired = false;
    private bool bIsPlayerActive = false;
    private bool bIsFocusing = false;
    private bool bIsFreeCam = false;

    [SerializeField]
    private bool bCanFreeCam = true;

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
        PLAYER_MOUSE_SENSITIVITY = PlayerOptions.GetGunSensitivity();
    }

    private void Start()
    {
        InputManager.Instance.OnFocusAction += InputManager_OnFocus;
        OptionsManager.OnGunSensitivityUpdated += OnSensitivityUpdated;
        TutorialUI.OnDisplayTutorial += TutorialUI_OnDisplayTutorial;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnFocusAction -= InputManager_OnFocus;
        OptionsManager.OnGunSensitivityUpdated -= OnSensitivityUpdated;
        TutorialUI.OnDisplayTutorial -= TutorialUI_OnDisplayTutorial;
    }

    void Update()
    {
        if (!bIsPlayerActive && !bBulletFired)
        {
            return;
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
        if (!bCanRotate)
        {
            return;
        }

        Vector2 mouseMovement =
            InputManager.Instance.GetMouseMovement() * mouseSensitivity * Time.deltaTime;

        mouseMovement.x *= PLAYER_MOUSE_SENSITIVITY;
        mouseMovement.y *= PLAYER_MOUSE_SENSITIVITY;

        xRotation -= mouseMovement.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseMovement.x);
    }

    private void ToggleFreeCamMode(bool toggle)
    {
        bIsFreeCam = toggle;

        if (bIsFreeCam)
        {
            InputManager.Instance.OnFreeCamAction -= InputManager_OnFreeCamAction;
            InputManager.Instance.OnFreeCamPossessAction += InputManager_OnPossessAction;
            BulletVelocityUI.Instance.ToggleUIActive(false);
        }
        else
        {
            InputManager.Instance.OnFreeCamAction += InputManager_OnFreeCamAction;
            InputManager.Instance.OnFreeCamPossessAction -= InputManager_OnPossessAction;
            BulletVelocityUI.Instance.ToggleUIActive(true);
        }
    }

    public void TogglePlayerController(bool toggle)
    {
        bIsPlayerActive = toggle;
        bIsFocusing = false;

        if (bIsPlayerActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
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
            Cursor.lockState = CursorLockMode.None;
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
            InputManager.Instance.OnShootAction += InputManager_OnStartLockOnAction;
            InputManager.Instance.OnShootReleaseAction += InputManager_OnRedirect;
            InputManager.Instance.OnPossessAction += InputManager_OnPossessAction;

            if (bCanFreeCam)
            {
                InputManager.Instance.OnFreeCamAction += InputManager_OnFreeCamAction;
            }

            if (bCanRedirect)
            {
                RedirectManager.Instance.ToggleRedirectUI(true);
            }

            BulletVelocityUI.Instance.ToggleUIActive(true);
        }
        else
        {
            if (bIsFreeCam)
            {
                ToggleFreeCamMode(false);
            }

            InputManager.Instance.OnShootAction -= InputManager_OnStartLockOnAction;
            InputManager.Instance.OnShootReleaseAction -= InputManager_OnRedirect;
            InputManager.Instance.OnPossessAction -= InputManager_OnPossessAction;

            if (bCanFreeCam)
            {
                InputManager.Instance.OnFreeCamAction -= InputManager_OnFreeCamAction;
            }

            OnPlayerStateChanged?.Invoke(this, 0);
            RedirectManager.Instance.ToggleRedirectUI(false);
            BulletVelocityUI.Instance.ToggleUIActive(false);
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

    private void InputManager_OnStartLockOnAction()
    {
        if (!bIsFocusing || !bCanRedirect)
        {
            return;
        }

        bulletPossessor.LockOnBullet();
    }

    private void InputManager_OnRedirect()
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

        if (bulletPossessor.TryPossess())
        {
            ToggleFreeCamMode(false);
        }
    }

    private void InputManager_OnFreeCamAction()
    {
        if (!GameManager.Instance.IsLevelActive() || !bCanPossess)
        {
            return;
        }
        bulletPossessor.PossessFreeCamBullet();
        ToggleFreeCamMode(true);
    }

    private void OnSensitivityUpdated(object sender, float newSensitivity)
    {
        PLAYER_MOUSE_SENSITIVITY = newSensitivity;
    }

    private void TutorialUI_OnDisplayTutorial(object sender, bool toggle)
    {
        TogglePlayerController(!toggle);
    }

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
