using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlayerFamineAbility : MonoBehaviour
{
    private int lastRandomVoiceline = 0;
    private BulletPossessor bulletPossessor;

    [SerializeField]
    private AudioClip abilitySFX;

    [SerializeField]
    private AudioClip abilityNoTargetSFX;

    [SerializeField]
    private AudioClip[] famineVoicelines;

    [SerializeField]
    private string[] famineLineText;

    [SerializeField]
    private AudioSource famineAudioSource;
    public static event Action OnAbilityUIUsed;
    public static EventHandler<CutInType> OnFamineAbility;
    public static EventHandler<string> OnFamineAbilityText;

    private void Start()
    {
        bulletPossessor = GetComponent<BulletPossessor>();
        InputManager.Instance.OnFamineAction += TryUseAbility;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnFamineAction -= TryUseAbility;
    }

    private void TryUseAbility()
    {
        BulletPossessTarget centralBullet = bulletPossessor.GetCentreOfScreenPossessable();

        if (!centralBullet)
        {
            //Play SFX for no available bullet
            AudioManager.PlaySFX(
                abilityNoTargetSFX,
                0.8f,
                3,
                Camera.main.transform.position,
                false
            );
            return;
        }

        OnAbilityUIUsed?.Invoke();

        PlayAbilityAudio();

        // Activate famine flair
        OnFamineAbility?.Invoke(this, CutInType.Famine);

        centralBullet.KillBullet();
        RedirectManager.Instance.IncrementRedirects();

        FamineAbility.FamineAbilityUsed(this);
    }

    private void PlayAbilityAudio()
    {
        AudioManager.PlaySFX(abilitySFX, 0.5f, 3, Camera.main.transform.position, false);

        int randomInt = Random.Range(0, famineVoicelines.Length);

        int retries = 3;
        int counter = 0;

        while (counter < retries)
        {
            if (randomInt != lastRandomVoiceline)
            {
                break;
            }

            randomInt = Random.Range(0, famineVoicelines.Length);
            counter++;
        }

        // while (randomInt == lastRandomVoiceline)
        // {
        //     randomInt = Random.Range(0, famineVoicelines.Length);
        // }

        lastRandomVoiceline = randomInt;

        famineAudioSource.clip = famineVoicelines[randomInt];
        famineAudioSource.transform.position = Camera.main.transform.position;
        famineAudioSource.Play();

        if (randomInt < famineLineText.Length)
        {
            OnFamineAbilityText.Invoke(this, famineLineText[randomInt]);
        }
    }

    public void UndoAbility()
    {
        RedirectManager.Instance.DecrementRedirects();
    }
}
