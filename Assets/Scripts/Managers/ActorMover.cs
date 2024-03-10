using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMover : MonoBehaviour
{
    public void MoveActor(
        ActorMovementSO actorMovementSO,
        IMover actorMover,
        Action onMovementComplete
    ) { }
}
