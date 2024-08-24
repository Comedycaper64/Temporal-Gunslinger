using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimatorMapper : MonoBehaviour
{
    private Dictionary<RuntimeAnimatorController, Animator[]> actorAnimatorPair =
        new Dictionary<RuntimeAnimatorController, Animator[]>();

    public Animator[] GetAnimators(RuntimeAnimatorController actorController)
    {
        Animator[] actorAnimators;
        if (!actorAnimatorPair.TryGetValue(actorController, out actorAnimators))
        {
            //Finds Gameobject with the animator controller, adds to dictionary
            Animator[] animators = FindObjectsOfType<Animator>();
            //Debug.Log("Animators found: " + animators.Length);
            Animator[] desiredAnimators = Array.FindAll(
                animators,
                animator => animator.runtimeAnimatorController == actorController
            );
            if (desiredAnimators.Length == 0)
            {
                Debug.Log("Animator not found");
                return new Animator[0];
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
