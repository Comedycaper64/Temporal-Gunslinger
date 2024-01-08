using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : RewindableMovement
{
    // [SerializeField]
    // private bool startOnSpawn;

    [SerializeField]
    private float bulletStartSpeed = 1f;
    private float bulletSpeed = 0f;
    private Vector3 flightDirection;

    [SerializeField]
    private Transform bulletModel;

    // private void Start()
    // {
    //     if (startOnSpawn)
    //     {
    //         ToggleBulletMovement(true);
    //     }
    // }

    private void StartBullet()
    {
        bulletSpeed = bulletStartSpeed;
    }

    private void StopBullet()
    {
        bulletSpeed = 0f;
    }

    public void ToggleBulletMovement(bool toggle)
    {
        if (toggle)
        {
            StartBullet();
        }
        else
        {
            StopBullet();
        }
    }

    public void RedirectBullet(Vector3 newDirection, Quaternion newRotation)
    {
        //Rudimentary redirect, uses rotation of camera
        flightDirection = newDirection;
        bulletModel.rotation = newRotation;
    }

    public Vector3 GetFlightDirection()
    {
        return flightDirection;
    }

    public Vector3 GetBulletUp()
    {
        return bulletModel.up;
    }

    private void Update()
    {
        //transform.Translate(flightDirection * bulletSpeed * Time.deltaTime);
        transform.position += flightDirection * bulletSpeed * Time.deltaTime;
    }

    public override void BeginRewind()
    {
        bulletSpeed = Mathf.Abs(bulletSpeed) * -1;
        bulletStartSpeed = Mathf.Abs(bulletStartSpeed) * -1;
    }

    public override void BeginPlay()
    {
        bulletSpeed = Mathf.Abs(bulletSpeed);
        bulletStartSpeed = Mathf.Abs(bulletStartSpeed);
    }
}
