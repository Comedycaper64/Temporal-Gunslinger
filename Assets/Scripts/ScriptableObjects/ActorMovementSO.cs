using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorMovement", menuName = "ActorMovementSO", order = 0)]
public class ActorMovementSO : CinematicNode
{
    public ActorSO actor;
    public int actorIndex;
    public Vector3 movement;
    public float movementSpeed;
    public bool playMovementUnInterrupted;
}
