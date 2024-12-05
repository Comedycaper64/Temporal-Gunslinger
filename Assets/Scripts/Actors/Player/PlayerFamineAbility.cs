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
    private AudioClip[] famineVoicelines;

    [SerializeField]
    private AudioSource famineAudioSource;
    public static event Action OnAbilityUIUsed;
    public static EventHandler<CutInType> OnFamineAbility;

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

        while (randomInt == lastRandomVoiceline)
        {
            randomInt = Random.Range(0, famineVoicelines.Length);
        }

        lastRandomVoiceline = randomInt;

        famineAudioSource.clip = famineVoicelines[randomInt];
        famineAudioSource.transform.position = Camera.main.transform.position;
        famineAudioSource.Play();

        // AudioManager.PlaySFX(
        //     famineVoicelines[randomInt],
        //     0.75f,
        //     0,
        //     Camera.main.transform.position,
        //     false,
        //     false
        // );
    }

    public void UndoAbility()
    {
        RedirectManager.Instance.DecrementRedirects();
    }
}
