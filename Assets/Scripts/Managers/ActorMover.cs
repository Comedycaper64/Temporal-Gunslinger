using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMover : MonoBehaviour
{
    private Vector3 destination;
    private float movementSpeed;
    private int currentMovementType = 0;
    private int currentIdleType = 0;

    [SerializeField]
    private Transform movingTransform;
    private bool movementRequired = false;
    private bool shouldGoIdle = false;
    private Action onMovementComplete;

    private Animator actorAnimator;

    [SerializeField]
    private AnimationClip[] movingAnimations;

    [SerializeField]
    private AnimationClip[] idleAnimations;

    private void Awake()
    {
        actorAnimator = GetComponent<Animator>();
    }

    public void MoveActor(ActorMovementSO actorMovementSO, Action onMovementComplete)
    {
        currentMovementType = actorMovementSO.movementType;
        currentIdleType = actorMovementSO.idleType;
        SetMovementTarget(actorMovementSO.movement, actorMovementSO.movementSpeed);
        shouldGoIdle = actorMovementSO.goIdleAtEnd;
        if (actorMovementSO.playMovementUnInterrupted)
        {
            this.onMovementComplete = onMovementComplete;
        }
        else
        {
            onMovementComplete();
        }
    }

    private void SetMovementTarget(Vector3 movement, float speed)
    {
        movementRequired = true;
        destination = movingTransform.position + movement;
        //this.movement = movement.normalized * speed;
        movementSpeed = speed;
        if (movingAnimations == null)
        {
            return;
        }

        Debug.Log(movingAnimations[currentMovementType].name);
        Debug.Log(currentMovementType);

        actorAnimator.CrossFadeInFixedTime(movingAnimations[currentMovementType].name, 0.1f);
        actorAnimator.SetBool("moving", true);
    }

    private void FinishMovement()
    {
        movementRequired = false;

        if (onMovementComplete != null)
        {
            onMovementComplete();
        }
        onMovementComplete = null;

        if ((idleAnimations == null) || !shouldGoIdle)
        {
            return;
        }
        actorAnimator.CrossFadeInFixedTime(idleAnimations[currentIdleType].name, 0.1f);
        actorAnimator.SetBool("moving", false);
    }

    private void Update()
    {
        if (movementRequired)
        {
            //movingTransform.position += movement * Time.deltaTime;
            movingTransform.position = Vector3.MoveTowards(
                movingTransform.position,
                destination,
                movementSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, destination) < 0.5f)
            {
                FinishMovement();
            }
        }
    }
}
