using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMover : MonoBehaviour
{
    private Vector3 destination;
    private float movementSpeed;

    [SerializeField]
    private Transform movingTransform;
    private bool movementRequired = false;
    private bool shouldGoIdle = false;
    private Action onMovementComplete;

    private Animator actorAnimator;

    [SerializeField]
    private AnimationClip movingAnimation;

    [SerializeField]
    private AnimationClip idleAnimation;

    private void Awake()
    {
        actorAnimator = GetComponent<Animator>();
    }

    public void MoveActor(ActorMovementSO actorMovementSO, Action onMovementComplete)
    {
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
        if (!movingAnimation)
        {
            return;
        }
        actorAnimator.CrossFadeInFixedTime(movingAnimation.name, 0.1f);
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

        if (!idleAnimation || !shouldGoIdle)
        {
            return;
        }
        actorAnimator.CrossFadeInFixedTime(idleAnimation.name, 0.1f);
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
