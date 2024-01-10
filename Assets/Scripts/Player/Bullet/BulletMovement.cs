using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : RewindableMovement
{
    private bool bShouldRotate;
    private float rotationTimer;

    private float rotationSpeed = 2.5f;
    private Vector3 flightDirection;
    private Quaternion targetRotation;

    [SerializeField]
    private Transform bulletModel;

    private void Update()
    {
        transform.position += flightDirection * speed * Time.deltaTime;

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

    public void RedirectBullet(Vector3 newDirection, Quaternion newRotation)
    {
        //Rudimentary redirect, uses rotation of camera
        flightDirection = newDirection;
        //bulletModel.rotation = newRotation;
        targetRotation = newRotation;
        rotationTimer = 0f;
        bShouldRotate = true;
    }

    public Vector3 GetFlightDirection()
    {
        return flightDirection;
    }

    public Vector3 GetBulletUp()
    {
        return bulletModel.up;
    }
}
