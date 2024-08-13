using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorIndexer : MonoBehaviour
{
    [SerializeField]
    private int actorIndex = 0;

    public int GetActorIndex()
    {
        return actorIndex;
    }
}
