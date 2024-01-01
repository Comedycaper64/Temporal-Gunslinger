using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField]
    private float bulletStartSpeed = 1f;
    private float bulletSpeed = 0f;

    [SerializeField]
    private Transform bulletModel;

    private void Start()
    {
        SetupBullet(bulletStartSpeed);
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

    private void Update()
    {
        transform.Translate(bulletModel.forward * bulletSpeed * Time.deltaTime);
    }
}
