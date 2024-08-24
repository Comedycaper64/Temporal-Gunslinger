using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Actor", menuName = "Cinematic Node/ActorSO", order = 0)]
public class ActorSO : ScriptableObject
{
    [SerializeField]
    private string actorName;

    [SerializeField]
    private AudioClip[] dialogueNoises;

    [SerializeField]
    private Sprite[] actorSprites;

    [SerializeField]
    private float spriteSwitchTime = 1f;

    [SerializeField]
    private RuntimeAnimatorController animatorController;

    public string GetActorName()
    {
        return actorName;
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
}
