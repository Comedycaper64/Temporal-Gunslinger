using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform bulletCamera;

    public Quaternion GetCameraForward()
    {
        return Quaternion.LookRotation(bulletCamera.forward);
    }
}
