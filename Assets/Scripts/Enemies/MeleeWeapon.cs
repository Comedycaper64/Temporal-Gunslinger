using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private bool bWeaponActive;

    [SerializeField]
    private RewindableMovement movement;

    public void WeaponImpact()
    {
        //movement.ToggleMovement(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!bWeaponActive)
        // {
        //     return;
        // }

        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(this);
        }
    }
}
