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
