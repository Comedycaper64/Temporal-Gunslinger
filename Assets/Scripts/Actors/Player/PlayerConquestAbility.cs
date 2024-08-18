using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerConquestAbility : MonoBehaviour
{
    private bool bAbilityUsed = true;
    private bool spawned = false;
    private int lastRandomVoiceline = 0;
    private float daggerSpawnDuration = 0.004f;
    private float daggerSpawnTimer = 0;
    private Vector3 currentPortalPoint;
    private Vector3 currentPortalDirection;
    private BulletPossessor bulletPossessor;
    private RewindState rewindState;

    [SerializeField]
    private GameObject conquestDagger;

    [SerializeField]
    private MeshRenderer daggerRenderer;

    [SerializeField]
    private GameObject conquestPortal;

    [SerializeField]
    private Material daggerSpawnMaterial;

    [SerializeField]
    private Material daggerFlyMaterial;

    [SerializeField]
    private AudioClip abilitySFX;

    [SerializeField]
    private AudioClip[] conquestVoicelines;

    public static EventHandler<CutInType> OnConquestAbility;

    private void Start()
    {
        bAbilityUsed = false;
        bulletPossessor = GetComponent<BulletPossessor>();
        rewindState = GetComponent<RewindState>();
        rewindState.ToggleMovement(true);
        InputManager.Instance.OnConquestAction += TryUseAbility;
    }

    private void Update()
    {
        if (bAbilityUsed)
        {
            daggerSpawnTimer += Time.deltaTime * rewindState.GetScaledSpeed();

            //Debug.Log(daggerSpawnTimer);

            if ((daggerSpawnTimer > daggerSpawnDuration) && (spawned == false))
            {
                spawned = true;
                daggerRenderer.material = daggerFlyMaterial;
            }
            else if ((daggerSpawnTimer < daggerSpawnDuration) && (spawned == true))
            {
                spawned = false;
                daggerRenderer.material = daggerSpawnMaterial;
                SetDaggerMaterialPoints();
            }
        }
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

        AudioManager.PlaySFX(abilitySFX, 0.5f, 3, Camera.main.transform.position, false);

        int randomInt = Random.Range(0, conquestVoicelines.Length);

        while (randomInt == lastRandomVoiceline)
        {
            randomInt = Random.Range(0, conquestVoicelines.Length);
        }

        lastRandomVoiceline = randomInt;

        AudioManager.PlaySFX(
            conquestVoicelines[randomInt],
            0.75f,
            0,
            Camera.main.transform.position,
            false
        );

        // Activate conquest flair
        OnConquestAbility?.Invoke(this, CutInType.Conquest);

        daggerSpawnTimer = 0f;
        spawned = false;

        bAbilityUsed = true;
    }

    private void SetDaggerMaterialPoints()
    {
        Material daggerMaterial = daggerRenderer.material;
        daggerMaterial.SetVector("_Portal_Point", currentPortalPoint);
        daggerMaterial.SetVector("_Portal_Direction", currentPortalDirection);
    }

    private void SpawnDagger(BulletPossessTarget activeBullet)
    {
        Vector3 activeBulletPosition = activeBullet.transform.position;

        Vector3 cameraDistance = Camera.main.transform.position - activeBulletPosition;

        Vector3 daggerSpawnPoint = activeBulletPosition + (-cameraDistance / 2);

        Quaternion daggerSpawnRotation = Quaternion.LookRotation(Camera.main.transform.forward);

        currentPortalPoint = daggerSpawnPoint;

        currentPortalDirection = Camera.main.transform.forward;

        SetDaggerMaterialPoints();

        //Spawn Portal object
        Factory.InstantiateGameObject(conquestPortal, daggerSpawnPoint, daggerSpawnRotation);

        BulletDeadState.bulletNumber++;

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
        BulletDeadState.bulletNumber--;
    }
}
