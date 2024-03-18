using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : RewindableMovement
{
    private bool bShouldRotate;
    private float rotationTimer;
    private float rotationSpeed = 2.5f;
    private float spinSpeed = 150f;
    private float dropVelocity = 50f;
    public float velocityLossRate = 5f;
    private Vector3 flightDirection;
    private Quaternion targetRotation;

    [SerializeField]
    private Transform bulletModel;

    [SerializeField]
    private GameObject redirectVFXPrefab;

    private RedirectManager redirectManager;

    private void Start()
    {
        redirectManager = RedirectManager.Instance;
    }

    private void Update()
    {
        transform.position += flightDirection * GetSpeed() * Time.deltaTime;

        SpinBullet();

        if (!bShouldRotate)
        {
            return;
        }

        bulletModel.rotation = Quaternion.Slerp(
            bulletModel.rotation,
            targetRotation,
            rotationTimer * rotationSpeed
        );
        rotationTimer += Time.deltaTime;

        if (Quaternion.Angle(bulletModel.rotation, targetRotation) < 0.1f)
        {
            bShouldRotate = false;
        }
    }

    private void SpinBullet()
    {
        bulletModel.eulerAngles += new Vector3(0f, 0f, spinSpeed * Time.deltaTime);
    }

    public void RedirectBullet(Vector3 newDirection, Quaternion newRotation)
    {
        if (redirectManager.TryRedirect())
        {
            Redirect.BulletRedirected(transform.position, GetFlightDirection(), this, 1f, false);
            Factory.InstantiateGameObject(
                redirectVFXPrefab,
                transform.position,
                Quaternion.LookRotation(GetFlightDirection())
            );
            ChangeTravelDirection(newDirection, newRotation);
        }
    }

    public void ChangeTravelDirection(Vector3 newDirection, Quaternion newRotation)
    {
        flightDirection = newDirection;
        targetRotation = newRotation;
        rotationTimer = 0f;
        bShouldRotate = true;
    }

    public void RicochetBullet(Collision hitObject, float velocityAugment)
    {
        Vector3 hitNormal = hitObject.GetContact(0).normal.normalized;

        // Vector3 testNormal = (
        //     transform.position - hitObject.ClosestPoint(transform.position)
        // ).normalized;
        Vector3 flightNormalized = GetFlightDirection().normalized;
        //float flightSpeed = GetFlightDirection().magnitude;
        //Debug.Log("Flight Direction: " + GetFlightDirection());

        Vector3 ricochetDirection =
            2 * Vector3.Dot(-flightNormalized, hitNormal) * (hitNormal + flightNormalized);
        //Debug.Log("Ricochet Direction: " + ricochetDirection);

        Redirect.BulletRedirected(
            transform.position,
            GetFlightDirection(),
            this,
            velocityAugment,
            true
        );

        AugmentVelocity(velocityAugment);
        ChangeTravelDirection(ricochetDirection, Quaternion.LookRotation(ricochetDirection));
    }

    public void SlowBullet(float velocityAugment)
    {
        Redirect.BulletRedirected(
            transform.position,
            GetFlightDirection(),
            this,
            velocityAugment,
            true
        );

        AugmentVelocity(velocityAugment);
    }

    public Vector3 GetFlightDirection()
    {
        return flightDirection;
    }

    public float GetVelocity()
    {
        return Mathf.Abs(GetUnscaledSpeed());
    }

    public void AugmentVelocity(float velocityMultiplier)
    {
        float speed = GetUnscaledSpeed();
        SetSpeed(speed *= velocityMultiplier);
        //rewindable action for undoing augmention
    }

    public void LoseVelocity()
    {
        float speed = GetUnscaledSpeed();
        SetSpeed(speed -= velocityLossRate * Time.deltaTime);
    }

    public void ApplyGravity()
    {
        if (transform.position.y <= 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            return;
        }

        transform.position += new Vector3(0f, -GetSpeed() * Time.deltaTime, 0f);
    }

    public bool ShouldBulletDrop()
    {
        return Mathf.Abs(GetUnscaledSpeed()) < dropVelocity;
    }

    public bool ShouldBulletStop()
    {
        return Mathf.Abs(GetUnscaledSpeed()) < 10f;
    }

    public void ToggleBulletModel(bool toggle)
    {
        bulletModel.gameObject.SetActive(toggle);
    }

    public void UndoRedirect(
        Vector3 position,
        Vector3 direction,
        float velocityAugment,
        bool bIsRicochet
    )
    {
        Quaternion undoRotation = Quaternion.LookRotation(direction);
        ChangeTravelDirection(direction, undoRotation);
        transform.position = position;
        float speed = GetUnscaledSpeed();
        SetSpeed(speed /= velocityAugment);
        if (!bIsRicochet)
        {
            redirectManager.IncrementRedirects();
        }
    }
}
