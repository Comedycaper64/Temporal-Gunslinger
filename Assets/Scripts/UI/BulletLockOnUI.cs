using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class BulletLockOnUI : MonoBehaviour
{
    private bool uiActive = false;
    private bool lockingOn = false;
    private bool charging = false;
    private float tweenTimer = 0f;
    private float chargeTweenTimer = 0f;
    private LockOnTarget currentTarget;
    private BulletPossessTarget raycastOrigin;

    [SerializeField]
    private LayerMask lockOnLayermask;

    [SerializeField]
    private CanvasGroupFader lockOnUI;

    [SerializeField]
    private CanvasGroupFader chargeUI;

    [SerializeField]
    private GameObject lockOnIcon;

    [SerializeField]
    private AnimationCurve lockOnScaleCurve;

    [SerializeField]
    private MMF_Player lockOnFeedback;

    [SerializeField]
    private MMF_Player chargeFeedback;

    [SerializeField]
    private AudioClip lockOnSFX;

    [SerializeField]
    private AudioClip lockOnChargedSFX;

    private void Awake()
    {
        lockOnUI.SetCanvasGroupAlpha(0f);

        BulletLockOn.OnLockOnUI += ToggleLockingOn;
        FocusManager.OnFocusToggle += ToggleUI;
        BulletPossessor.OnNewBulletPossessed += SetNewRaycastOrigin;
        BulletPossessor.OnBulletCharging += ToggleCharging;
        // GameManager.OnGameStateChange += ToggleUI;
        // RewindManager.OnRewindToStart += TurnOffUI;
    }

    private void OnDisable()
    {
        BulletLockOn.OnLockOnUI -= ToggleLockingOn;
        FocusManager.OnFocusToggle -= ToggleUI;
        BulletPossessor.OnNewBulletPossessed -= SetNewRaycastOrigin;
        BulletPossessor.OnBulletCharging -= ToggleCharging;
        // GameManager.OnGameStateChange -= ToggleUI;
        // RewindManager.OnRewindToStart -= TurnOffUI;
    }

    private void Update()
    {
        if (lockingOn)
        {
            LockOnTarget();

            if (charging)
            {
                ChargingLockOn();
            }
        }
        else if (uiActive)
        {
            FindLockOnTargets();
        }
    }

    private void LockOnTarget()
    {
        if (!currentTarget || !currentTarget.GetTarget())
        {
            return;
        }

        Vector3 targetPosition = currentTarget.GetTarget().position;
        lockOnUI.transform.position = Camera.main.WorldToScreenPoint(targetPosition);

        if (lockOnUI.GetCanvasGroupAlpha() < 1f)
        {
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                1f,
                0.25f,
                1f,
                MMTween.MMTweenCurve.EaseInExponential
            );

            float lerpScale = lockOnScaleCurve.Evaluate(lerp);

            lockOnIcon.transform.localScale = new Vector3(lerpScale, lerpScale, lerpScale);

            lockOnUI.SetCanvasGroupAlpha(lerp);

            tweenTimer += Time.unscaledDeltaTime;

            if (lockOnUI.GetCanvasGroupAlpha() >= 1f)
            {
                AudioManager.PlaySFX(lockOnSFX, 0.5f, 0, Camera.main.transform.position, false);
                lockOnFeedback.PlayFeedbacks();
            }
        }
    }

    private void ChargingLockOn()
    {
        if (!currentTarget || !currentTarget.GetTarget())
        {
            return;
        }

        if (chargeUI.GetCanvasGroupAlpha() < 1f)
        {
            float lerp = MMTween.Tween(
                chargeTweenTimer,
                0f,
                1.5f,
                0f,
                1f,
                MMTween.MMTweenCurve.EaseInExponential
            );

            float lerpScale = lockOnScaleCurve.Evaluate(lerp);

            chargeUI.transform.localScale = new Vector3(lerpScale, lerpScale, lerpScale);

            chargeUI.SetCanvasGroupAlpha(lerp);

            chargeTweenTimer += Time.unscaledDeltaTime;

            if (chargeUI.GetCanvasGroupAlpha() >= 1f)
            {
                AudioManager.PlaySFX(
                    lockOnChargedSFX,
                    0.5f,
                    0,
                    Camera.main.transform.position,
                    false
                );
                chargeFeedback.PlayFeedbacks();
            }
        }
    }

    private void FindLockOnTargets()
    {
        if (!raycastOrigin)
        {
            return;
        }

        if (
            Physics.Raycast(
                raycastOrigin.transform.position,
                Camera.main.transform.forward,
                out RaycastHit hit,
                500f,
                lockOnLayermask
            )
        )
        {
            if (hit.transform.TryGetComponent<LockOnTarget>(out LockOnTarget lockOnTarget))
            {
                Vector3 targetPosition = lockOnTarget.GetTarget().position;
                lockOnUI.transform.position = Camera.main.WorldToScreenPoint(targetPosition);

                currentTarget = lockOnTarget;
                if (lockOnUI.GetCanvasGroupAlpha() <= 0f)
                {
                    lockOnUI.SetCanvasGroupAlpha(0.5f);
                    //lockOnUI.ToggleFade(true, 0.5f);
                }
            }
            else
            {
                currentTarget = null;
                lockOnUI.SetCanvasGroupAlpha(0f);
                //lockOnUI.ToggleFade(false);
            }
        }
        else
        {
            currentTarget = null;
            lockOnUI.SetCanvasGroupAlpha(0f);
            //lockOnUI.ToggleFade(false);
        }
    }

    private void SetNewRaycastOrigin(object sender, BulletPossessTarget possessTarget)
    {
        raycastOrigin = possessTarget;
    }

    private void ToggleUI(object sender, bool toggle)
    {
        uiActive = toggle;

        if (!toggle)
        {
            lockOnUI.SetCanvasGroupAlpha(0f);
            chargeUI.SetCanvasGroupAlpha(0f);
            ToggleLockingOn(this, false);
            //lockOnUI.ToggleFade(false);
        }
    }

    private void ToggleLockingOn(object sender, bool toggle)
    {
        tweenTimer = 0f;
        lockingOn = toggle;

        if (!lockOnFeedback.IsPlaying)
        {
            lockOnIcon.transform.localScale = Vector3.one;
        }
    }

    private void ToggleCharging(object sender, bool toggle)
    {
        chargeTweenTimer = 0f;
        charging = toggle;

        if (!chargeFeedback.IsPlaying)
        {
            chargeUI.transform.localScale = Vector3.one;
        }
    }
}
