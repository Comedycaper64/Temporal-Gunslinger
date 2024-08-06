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

    [SerializeField]
    private GameObject conquestPortal;

    private void Start()
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

        BulletPossessTarget activeBullet = bulletPossessor.GetPossessedBullet();
        if (!activeBullet)
        {
            return;
        }

        SpawnDagger(activeBullet);

        // UI change

        // Activate conquest flair

        bAbilityUsed = true;
    }

    private void SpawnDagger(BulletPossessTarget activeBullet)
    {
        Vector3 activeBulletPosition = activeBullet.transform.position;

        Vector3 cameraDistance = Camera.main.transform.position - activeBulletPosition;

        Vector3 daggerSpawnPoint = activeBulletPosition + (-cameraDistance / 2);

        Quaternion daggerSpawnRotation = Quaternion.LookRotation(Camera.main.transform.forward);

        //Spawn Portal object
        Factory.InstantiateGameObject(conquestPortal, daggerSpawnPoint, daggerSpawnRotation);

        //Make rewindable action for ability
        ConquestAbility.ConquestAbilityUsed(
            conquestDagger,
            daggerSpawnPoint,
            daggerSpawnRotation,
            bulletPossessor,
            activeBullet,
            this
        );
    }

    public void RefreshAbility()
    {
        bAbilityUsed = false;
    }
}
