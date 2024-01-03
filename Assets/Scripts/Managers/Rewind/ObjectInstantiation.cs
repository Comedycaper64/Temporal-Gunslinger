using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiation : RewindableAction
{
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

    public override void Undo()
    {
        GameObject.Destroy(createdGameObject);
    }
}
