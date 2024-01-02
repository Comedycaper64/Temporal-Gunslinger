using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour, IRewindable
{
    [SerializeField]
    private float bulletStartSpeed = 1f;
    private float bulletSpeed = 0f;

    [SerializeField]
    private Transform bulletModel;

    private void Start()
    {
        SetupBullet(bulletStartSpeed);
        RewindManager.Instance.OnRewindToggled += RewindToggled;
    }

    private void OnDisable()
    {
        RewindManager.Instance.OnRewindToggled -= RewindToggled;
    }

    private void RewindToggled(object sender, bool rewindToggled)
    {
        if (rewindToggled)
        {
            BeginRewind();
        }
        else
        {
            BeginPlay();
        }
    }

    public void SetupBullet(float speed)
    {
        bulletSpeed = speed;
    }

    public void RedirectBullet(Quaternion newRotation)
    {
        //Rudimentary redirect, uses rotation of camera
        bulletModel.rotation = newRotation;
    }

    public Quaternion GetRotation()
    {
        return bulletModel.rotation;
    }

    private void Update()
    {
        transform.Translate(bulletModel.forward * bulletSpeed * Time.deltaTime);
    }

    public void BeginRewind()
    {
        bulletSpeed = Mathf.Abs(bulletSpeed) * -1;
    }

    public void BeginPlay()
    {
        bulletSpeed = Mathf.Abs(bulletSpeed);
    }
}
