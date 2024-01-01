using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCameraController : MonoBehaviour
{
    // [SerializeField]
    // private float mouseSensitivity = 15f;
    // private float xRotation = 0f;

    [SerializeField]
    private Transform bulletCamera;
    private InputManager input;

    private void Start()
    {
        input = InputManager.Instance;
    }

    public Quaternion GetCameraRotation()
    {
        return bulletCamera.rotation;
    }

    private void Update()
    {
        // Vector2 mouseMovement = input.GetMouseMovement() * mouseSensitivity * Time.deltaTime;

        // xRotation -= mouseMovement.y;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // bulletCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
