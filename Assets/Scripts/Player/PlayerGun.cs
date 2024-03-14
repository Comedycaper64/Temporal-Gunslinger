using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private bool shouldGunMove;
    private float gunMoveTime = 0.1f;
    private float distanceAllowance = 0.001f;
    private Vector3 gunVelocity = Vector3.zero;
    private Transform target;

    [SerializeField]
    private Transform gunModel;

    [SerializeField]
    private Transform standbyPosition;

    [SerializeField]
    private Transform aimingPosition;

    [SerializeField]
    private Transform bulletPosition;

    [SerializeField]
    private VFXPlayback gunShotVFX;

    [SerializeField]
    private BulletStateMachine bulletStateMachine;
    private Bullet bullet;

    private void Start()
    {
        bullet = bulletStateMachine.GetComponent<Bullet>();
    }

    private void Update()
    {
        if (!shouldGunMove)
        {
            return;
        }

        gunModel.position = Vector3.SmoothDamp(
            gunModel.position,
            target.position,
            ref gunVelocity,
            gunMoveTime
        );

        if (Vector3.Distance(gunModel.position, target.position) < distanceAllowance)
        {
            shouldGunMove = false;
        }
    }

    public void ToggleAimGun(bool toggle)
    {
        if (toggle)
        {
            target = aimingPosition;
        }
        else
        {
            target = standbyPosition;
        }
        shouldGunMove = true;
    }

    public void FireGun()
    {
        //SFX
        gunShotVFX.PlayEffect();
        bulletStateMachine.SwitchToActive();
    }

    public void ResetBullet()
    {
        bullet.ResetBullet(bulletPosition);
    }

    // public void DisableBullet()
    // {
    //     bullet.SwitchToInactive();
    // }
}
