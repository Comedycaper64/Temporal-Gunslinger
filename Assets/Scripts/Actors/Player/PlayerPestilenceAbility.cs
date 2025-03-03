using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlayerPestilenceAbility : MonoBehaviour
{
    private int lastRandomVoiceline = 0;

    [SerializeField]
    private AudioClip abilitySFX;

    [SerializeField]
    private AudioClip[] pestilenceVoicelines;

    [SerializeField]
    private AudioSource pestilenceAudioSource;
    public static event Action OnAbilityUIUsed;
    public static EventHandler<CutInType> OnPestilenceAbility;

    private void OnEnable()
    {
        BulletBooster.OnBoost += AbilityFeedback;
    }

    private void OnDisable()
    {
        BulletBooster.OnBoost -= AbilityFeedback;
    }

    private void AbilityFeedback()
    {
        OnAbilityUIUsed?.Invoke();

        PlayAbilityAudio();

        OnPestilenceAbility?.Invoke(this, CutInType.Pestilence);
    }

    private void PlayAbilityAudio()
    {
        AudioManager.PlaySFX(abilitySFX, 0.6f, 0, Camera.main.transform.position, false);

        int randomInt = Random.Range(0, pestilenceVoicelines.Length);

        while (randomInt == lastRandomVoiceline)
        {
            randomInt = Random.Range(0, pestilenceVoicelines.Length);
        }

        lastRandomVoiceline = randomInt;

        pestilenceAudioSource.clip = pestilenceVoicelines[randomInt];
        pestilenceAudioSource.transform.position = Camera.main.transform.position;
        pestilenceAudioSource.Play();
    }
}
