using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnparentObject : RewindableAction
{
    private Transform unparentedGameObject;
    private Transform parentGameObject;

    public static void ObjectUnparented(Transform unparentedGameObject, Transform parentGameObject)
    {
        UnparentObject unparentObject = new UnparentObject(unparentedGameObject, parentGameObject);
    }

    public UnparentObject(Transform unparentedGameObject, Transform parentGameObject)
    {
        this.unparentedGameObject = unparentedGameObject;
        this.parentGameObject = parentGameObject;
        unparentedGameObject.SetParent(null);
        Execute();
    }

    public override void Undo()
    {
        unparentedGameObject.SetParent(parentGameObject);
    }
}
