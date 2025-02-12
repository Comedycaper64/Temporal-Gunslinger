using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletBooster : MonoBehaviour, IReactable
{
    private float boostSpeed = 600f;
    private float boostVelocityLoss = 300f;
    private BulletMovement bulletMovement;

    [SerializeField]
    private GameObject boostEffect;

    [SerializeField]
    private GameObject boostCrystals;
    public static Action OnBoost;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
        bulletMovement.OnSlowed += RemoveBoostEffect;
    }

    private void OnDisable()
    {
        bulletMovement.OnSlowed -= RemoveBoostEffect;
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
        boostEffect.SetActive(true);
        boostCrystals.SetActive(true);

        OnBoost?.Invoke();
    }

    private void RemoveBoostEffect()
    {
        boostEffect.SetActive(false);
        StartReaction.ReactionStarted(this);
    }

    public void UndoBoost(float initialSpeed, float initialVelocityLoss)
    {
        RedirectManager.Instance.IncrementRedirects();

        bulletMovement.SetSpeed(-initialSpeed);
        bulletMovement.velocityLossRate = initialVelocityLoss;
        boostEffect.SetActive(false);
        boostCrystals.SetActive(false);
    }

    public void UndoReaction()
    {
        boostEffect.SetActive(true);
    }
}
