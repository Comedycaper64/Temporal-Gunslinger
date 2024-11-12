using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFreeCamMovement : MonoBehaviour
{
    private bool bCanMove = false;
    private float moveSpeed = 3f;
    private InputManager inputManager;

    // [SerializeField]
    // private Transform cameraTransform;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        if (!bCanMove)
        {
            return;
        }

        transform.position +=
            (
                Camera.main.transform.right * inputManager.GetFreeCamMovement().x
                + Camera.main.transform.forward * inputManager.GetFreeCamMovement().y
            )
            * moveSpeed
            * Time.unscaledDeltaTime;
    }

    public void ToggleCamMovement(bool toggle)
    {
        bCanMove = toggle;
    }
}
