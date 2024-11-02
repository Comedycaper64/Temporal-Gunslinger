using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
[CreateAssetMenu(fileName = "Actor", menuName = "Cinematic Node/ActorSO", order = 0)]
public class ActorSO : ScriptableObject
{
    [SerializeField]
    private string actorName;

    [ColorUsage(true, true)]
    [SerializeField]
    private Color actorNameColour;

    [SerializeField]
    private AudioClip[] dialogueNoises;

    [SerializeField]
    private Sprite[] actorSprites;

    [SerializeField]
    private float spriteSwitchTime = 1f;

    [SerializeField]
    private RuntimeAnimatorController animatorController;

    [SerializeField]
    private AudioMixerGroup actorAudioMixer;

    public string GetActorName()
    {
        return actorName;
    }

    public Color GetActorNameColour()
    {
        return actorNameColour;
    }

    public AudioClip[] GetDialogueNoises()
    {
        return dialogueNoises;
    }

    public Sprite[] GetActorSprites()
    {
        return actorSprites;
    }

    public float GetSpriteSwitchTime()
    {
        return spriteSwitchTime;
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
        return animatorController;
    }

    public AudioMixerGroup GetAudioMixer()
    {
        return actorAudioMixer;
    }
}
