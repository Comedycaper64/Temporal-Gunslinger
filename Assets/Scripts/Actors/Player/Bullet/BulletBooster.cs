using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletBooster : MonoBehaviour
{
    private float boostSpeed = 600f;
    private float boostVelocityLoss = 300f;
    private BulletMovement bulletMovement;
    public static Action OnBoost;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
    }

    public void CrystalBoost()
    {
        RedirectManager.Instance.TryRedirect();
        PestilenceAbility.BulletBoosted(
            this,
            bulletMovement.GetVelocity(),
            bulletMovement.velocityLossRate
        );

        bulletMovement.SetSpeed(boostSpeed);
        bulletMovement.velocityLossRate = boostVelocityLoss;

        OnBoost?.Invoke();
    }

    public void UndoBoost(float initialSpeed, float initialVelocityLoss)
    {
        RedirectManager.Instance.IncrementRedirects();

        bulletMovement.SetSpeed(-initialSpeed);
        bulletMovement.velocityLossRate = initialVelocityLoss;
    }
}
