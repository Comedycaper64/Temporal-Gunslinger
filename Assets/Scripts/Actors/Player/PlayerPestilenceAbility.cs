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
    private string[] pestilenceLineText;

    [SerializeField]
    private AudioSource pestilenceAudioSource;
    public static event Action OnAbilityUIUsed;
    public static EventHandler<CutInType> OnPestilenceAbility;
    public static EventHandler<string> OnPestilenceAbilityText;

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

        int retries = 3;
        int counter = 0;

        while (counter < retries)
        {
            if (randomInt != lastRandomVoiceline)
            {
                break;
            }

            randomInt = Random.Range(0, pestilenceVoicelines.Length);
            counter++;
        }

        // while (randomInt == lastRandomVoiceline)
        // {
        //     randomInt = Random.Range(0, pestilenceVoicelines.Length);
        // }

        lastRandomVoiceline = randomInt;

        pestilenceAudioSource.clip = pestilenceVoicelines[randomInt];
        pestilenceAudioSource.transform.position = Camera.main.transform.position;
        pestilenceAudioSource.Play();

        if (randomInt < pestilenceLineText.Length)
        {
            OnPestilenceAbilityText?.Invoke(this, pestilenceLineText[randomInt]);
        }
    }
}
