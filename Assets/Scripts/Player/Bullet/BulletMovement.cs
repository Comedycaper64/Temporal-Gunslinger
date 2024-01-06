using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : RewindableMovement
{
    [SerializeField]
    private bool startOnSpawn;

    [SerializeField]
    private float bulletStartSpeed = 1f;
    private float bulletSpeed = 0f;

    [SerializeField]
    private Transform bulletModel;

    private void Start()
    {
        if (startOnSpawn)
        {
            ToggleBulletMovement(true);
        }
    }

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

    public void RedirectBullet(Quaternion newForward)
    {
        //Rudimentary redirect, uses rotation of camera
        //bulletModel.localRotation = newRotation;
    }

    public Quaternion GetBulletRotation()
    {
        return bulletModel.rotation;
    }

    private void Update()
    {
        transform.Translate(bulletModel.forward * bulletSpeed * Time.deltaTime);
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
