using System;
using UnityEngine;

public class BulletBooster : MonoBehaviour, IReactable
{
    private bool hasBoosted = false;
    private float boostSpeed = 600f;
    private float boostVelocityLoss = 300f;
    private BulletMovement bulletMovement;

    [SerializeField]
    private GameObject boostEffect;

    [SerializeField]
    private GameObject boostCrystals;
    public static Action OnBoost;
    public static Action OnBoostACH;

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

        float bulletVelocity = bulletMovement.GetVelocity();
        PestilenceAbility.BulletBoosted(this, bulletVelocity, bulletMovement.velocityLossRate);

        if ((bulletVelocity > 0f) && bulletMovement.ShouldBulletDrop())
        {
            OnBoostACH?.Invoke();
        }

        bulletMovement.SetSpeed(boostSpeed);
        bulletMovement.velocityLossRate = boostVelocityLoss;
        boostEffect.SetActive(true);
        boostCrystals.SetActive(true);
        hasBoosted = true;

        OnBoost?.Invoke();
    }

    private void RemoveBoostEffect()
    {
        if (boostEffect.activeInHierarchy)
        {
            boostEffect.SetActive(false);
            StartReaction.ReactionStarted(this);
        }
    }

    public void UndoBoost(float initialSpeed, float initialVelocityLoss)
    {
        RedirectManager.Instance.IncrementRedirects();

        bulletMovement.SetSpeed(-initialSpeed);
        bulletMovement.velocityLossRate = initialVelocityLoss;
        boostEffect.SetActive(false);
        boostCrystals.SetActive(false);
        hasBoosted = false;
    }

    public bool HasBoosted()
    {
        return hasBoosted;
    }

    public void UndoReaction()
    {
        boostEffect.SetActive(true);
    }
}
