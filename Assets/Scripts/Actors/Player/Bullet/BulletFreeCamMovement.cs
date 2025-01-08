using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFreeCamMovement : MonoBehaviour
{
    private bool bCanMove = false;
    private float moveSpeed = 3f;
    private InputManager inputManager;

    [SerializeField]
    private Rigidbody freeCamrb;

    public static EventHandler<bool> OnFreeCamToggle;

    // [SerializeField]
    // private Transform cameraTransform;

    private void Start()
    {
        inputManager = InputManager.Instance;
        //freeCamrb.maxLinearVelocity = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (!bCanMove)
        {
            return;
        }

        freeCamrb.MovePosition(
            transform.position
                + (
                    (
                        Camera.main.transform.right * inputManager.GetFreeCamMovement().x
                        + Camera.main.transform.forward * inputManager.GetFreeCamMovement().y
                    )
                    * moveSpeed
                    * Time.fixedDeltaTime
                )
        );

        // //if (freeCamrb.)
        // freeCamrb.AddForce(
        //     (
        //         Camera.main.transform.right * inputManager.GetFreeCamMovement().x
        //         + Camera.main.transform.forward * inputManager.GetFreeCamMovement().y
        //     ) * Time.unscaledDeltaTime
        // );

        // transform.Translate(
        //     (
        //         (
        //             transform.right * inputManager.GetFreeCamMovement().x
        //             + transform.forward * inputManager.GetFreeCamMovement().y
        //         )
        //         * moveSpeed
        //         * Time.unscaledDeltaTime
        //     ),
        //     Camera.main.transform
        // );
        // transform.Translate(
        //     (
        //         Camera.main.transform.right * inputManager.GetFreeCamMovement().x
        //         + Camera.main.transform.forward * inputManager.GetFreeCamMovement().y
        //     )
        //         * moveSpeed
        //         * Time.unscaledDeltaTime
        // );

        // transform.position +=
        // (
        //     Camera.main.transform.right * inputManager.GetFreeCamMovement().x
        //     + Camera.main.transform.forward * inputManager.GetFreeCamMovement().y
        // )
        // * moveSpeed
        // * Time.unscaledDeltaTime;
    }

    public void ToggleCamMovement(bool toggle)
    {
        bCanMove = toggle;
        OnFreeCamToggle?.Invoke(this, toggle);
    }
}
