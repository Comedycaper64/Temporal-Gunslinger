using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerConquestAbility : MonoBehaviour
{
    public bool bCanUseAbility = true;
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

    // [SerializeField]
    // private MeshRenderer daggerRenderer;

    [SerializeField]
    private RewindableAnimator conquestPortal;

    [SerializeField]
    private GameObject daggerSpawnModel;

    private Renderer daggerSpawnRenderer;

    [SerializeField]
    private GameObject daggerFlyModel;

    [SerializeField]
    private AudioClip abilitySFX;

    [SerializeField]
    private AudioClip abilityUnavailableSFX;

    [SerializeField]
    private AudioSource conquestAudioSource;

    [SerializeField]
    private AudioClip[] conquestVoicelines;

    [SerializeField]
    private string[] conquestLineText;

    public static EventHandler<bool> OnAbilityUIUsed;
    public static EventHandler<CutInType> OnConquestAbility;
    public static EventHandler<string> OnConquestAbilityText;

    private void Start()
    {
        bAbilityUsed = false;
        bulletPossessor = GetComponent<BulletPossessor>();
        rewindState = GetComponent<RewindState>();
        rewindState.ToggleMovement(true);
        daggerSpawnRenderer = daggerSpawnModel.GetComponent<Renderer>();

        InputManager.Instance.OnConquestAction += TryUseAbility;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnConquestAction -= TryUseAbility;
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
                //daggerRenderer.material = daggerFlyMaterial;
                daggerFlyModel.SetActive(true);
                daggerSpawnModel.SetActive(false);
            }
            else if ((daggerSpawnTimer < daggerSpawnDuration) && (spawned == true))
            {
                spawned = false;
                //daggerRenderer.material = daggerSpawnMaterial;
                daggerFlyModel.SetActive(false);
                daggerSpawnModel.SetActive(true);
                SetDaggerMaterialPoints();
            }
        }
    }

    private void TryUseAbility()
    {
        if (!bCanUseAbility)
        {
            return;
        }

        if (bAbilityUsed)
        {
            //SFX for abiltiy used;
            AudioManager.PlaySFX(
                abilityUnavailableSFX,
                0.5f,
                0,
                Camera.main.transform.position,
                false
            );
            return;
        }

        BulletPossessTarget activeBullet = bulletPossessor.GetPossessedBullet();
        if (!activeBullet)
        {
            return;
        }

        SpawnDagger(activeBullet);

        // UI change
        OnAbilityUIUsed?.Invoke(this, true);

        AudioManager.PlaySFX(abilitySFX, 0.5f, 3, Camera.main.transform.position, false);

        int randomInt = Random.Range(0, conquestVoicelines.Length);

        int retries = 3;
        int counter = 0;

        while (counter < retries)
        {
            if (randomInt != lastRandomVoiceline)
            {
                break;
            }

            randomInt = Random.Range(0, conquestVoicelines.Length);
            counter++;
        }

        // while (randomInt == lastRandomVoiceline)
        // {
        //     randomInt = Random.Range(0, conquestVoicelines.Length);
        // }

        lastRandomVoiceline = randomInt;

        conquestAudioSource.clip = conquestVoicelines[randomInt];
        conquestAudioSource.transform.position = Camera.main.transform.position;
        conquestAudioSource.Play();

        // AudioManager.PlaySFX(
        //     conquestVoicelines[randomInt],
        //     0.75f,
        //     0,
        //     Camera.main.transform.position,
        //     false,
        //     false
        // );

        // Activate conquest flair
        OnConquestAbility?.Invoke(this, CutInType.Conquest);

        if (randomInt < conquestLineText.Length)
        {
            OnConquestAbilityText?.Invoke(this, conquestLineText[randomInt]);
        }

        daggerSpawnTimer = 0f;
        spawned = false;

        bAbilityUsed = true;
    }

    private void SetDaggerMaterialPoints()
    {
        Material daggerMaterial = daggerSpawnRenderer.material;
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
        //Factory.InstantiateGameObject(conquestPortal, daggerSpawnPoint, daggerSpawnRotation);
        conquestPortal.transform.SetPositionAndRotation(daggerSpawnPoint, daggerSpawnRotation);
        conquestPortal.gameObject.SetActive(true);
        conquestPortal.BeginPlay();

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
        conquestPortal.gameObject.SetActive(false);
        bAbilityUsed = false;
        BulletDeadState.bulletNumber--;
        OnAbilityUIUsed?.Invoke(this, false);
    }
}
