using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConquestAbility : MonoBehaviour
{
    private bool bAbilityUsed = true;
    private BulletPossessor bulletPossessor;

    [SerializeField]
    private GameObject conquestDagger;

    private void OnEnable()
    {
        bAbilityUsed = false;
        bulletPossessor = GetComponent<BulletPossessor>();
        InputManager.Instance.OnConquestAction += TryUseAbility;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnConquestAction -= TryUseAbility;
    }

    private void TryUseAbility()
    {
        if (bAbilityUsed)
        {
            //SFX for abiltiy used;
            return;
        }

        Transform activeBulletTransform = bulletPossessor.GetPossessedBulletTransform();
        if (!activeBulletTransform)
        {
            return;
        }

        SpawnDagger(activeBulletTransform);

        // UI change

        // Activate conquest flair

        bAbilityUsed = true;
    }

    private void SpawnDagger(Transform activeBulletTransform)
    {
        //Instantiate conquest dagger a bit in front of where the active bullet is in relation to the camera
        //Should fly forward in relation to camera
    }
}
