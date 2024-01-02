using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiation : IRewindableAction
{
    private float timestamp;
    private GameObject createdGameObject;

    public static void ObjectCreated(GameObject gameObject)
    {
        if (!RewindManager.Instance)
        {
            return;
        }

        ObjectInstantiation objectInstantiation = new ObjectInstantiation(gameObject);
    }

    public ObjectInstantiation(GameObject gameObject)
    {
        createdGameObject = gameObject;
        Execute();
    }

    public void Execute()
    {
        RewindManager.Instance.AddRewindable(this);
    }

    public float GetTimestamp()
    {
        return timestamp;
    }

    public void SetTimestamp(float timestamp)
    {
        this.timestamp = timestamp;
    }

    public void Undo()
    {
        GameObject.Destroy(createdGameObject);
    }
}
