using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform bulletCamera;
    private float bulletSpeed = 0f;

    public void SetupBullet(float speed)
    {
        bulletSpeed = speed;
    }

    public void RedirectBullet()
    {
        transform.rotation = bulletCamera.rotation;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
}
