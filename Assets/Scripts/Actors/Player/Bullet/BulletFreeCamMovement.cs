using System;
using UnityEngine;

public class BulletFreeCamMovement : MonoBehaviour
{
    private bool bCanMove = false;
    private float moveSpeed = 3f;
    private InputManager inputManager;

    [SerializeField]
    private Rigidbody freeCamrb;

    public static EventHandler<bool> OnFreeCamToggle;

    private void Start()
    {
        inputManager = InputManager.Instance;
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
    }

    public void ToggleCamMovement(bool toggle)
    {
        bCanMove = toggle;
        OnFreeCamToggle?.Invoke(this, toggle);
    }
}
