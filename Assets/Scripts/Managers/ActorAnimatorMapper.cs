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
            Debug.Log("Animators found: " + animators.Length);
            Animator[] desiredAnimators = Array.FindAll(
                animators,
                animator => animator.runtimeAnimatorController == actorController
            );
            if (desiredAnimators.Length == 0)
            {
                Debug.LogError("Animator not found");
                return null;
            }
            actorAnimators = new Animator[desiredAnimators.Length];
            foreach (Animator animator in desiredAnimators)
            {
                if (animator.TryGetComponent<ActorIndexer>(out ActorIndexer indexer))
                {
                    actorAnimators[indexer.GetActorIndex()] = animator;
                }
                else
                {
                    actorAnimators[0] = animator;
                }
            }
            actorAnimatorPair.Add(actorController, actorAnimators);
        }

        return actorAnimators;
    }
}