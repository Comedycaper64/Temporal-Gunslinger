using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Actor", menuName = "Temporal Gunslinger/ActorSO", order = 0)]
public class ActorSO : ScriptableObject
{
    [SerializeField]
    private string actorName;

    [SerializeField]
    private AudioClip[] dialogueNoises;

    [SerializeField]
    private AnimatorController animatorController;

    public string GetActorName()
    {
        return actorName;
    }

    public AudioClip[] GetDialogueNoises()
    {
        return dialogueNoises;
    }

    public AnimatorController GetAnimatorController()
    {
        return animatorController;
    }
}
