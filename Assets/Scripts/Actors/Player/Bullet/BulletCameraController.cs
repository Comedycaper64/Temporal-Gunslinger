using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform bulletCamera;

    public Quaternion GetCameraRotation()
    {
        return Quaternion.LookRotation(Camera.main.transform.forward);
    }

    public void ToggleCamera(bool toggle)
    {
        bulletCamera.gameObject.SetActive(toggle);
    }
}
