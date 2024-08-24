using UnityEngine;

[CreateAssetMenu(
    fileName = "ActorMovement",
    menuName = "Cinematic Node/ActorMovementSO",
    order = 0
)]
public class ActorMovementSO : CinematicNode
{
    public ActorSO actor;
    public int actorIndex;
    public int movementType = 0;
    public int idleType = 0;
    public Vector3 movement;
    public float movementSpeed;
    public bool playMovementUnInterrupted;
    public bool goIdleAtEnd;
}
