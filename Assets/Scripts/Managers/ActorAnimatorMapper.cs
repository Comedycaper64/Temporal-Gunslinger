using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ActorAnimatorMapper : MonoBehaviour
{
    private Dictionary<AnimatorController, Animator[]> actorAnimatorPair =
        new Dictionary<AnimatorController, Animator[]>();

    public Animator[] GetAnimators(AnimatorController actorController)
    {
        Animator[] actorAnimators;
        if (!actorAnimatorPair.TryGetValue(actorController, out actorAnimators))
        {
            //Finds Gameobject with the animator controller, adds to dictionary
            Animator[] animators = FindObjectsOfType<Animator>();
            Animator[] desiredAnimators = Array.FindAll(
                animators,
                animator => animator.runtimeAnimatorController == actorController
            );
            if (desiredAnimators.Length == 0)
            {
                Debug.LogError("Animator not found");
                return null;
            }
            actorAnimators = desiredAnimators;
            actorAnimatorPair.Add(actorController, actorAnimators);
        }

        return actorAnimators;
    }
}
