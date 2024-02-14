using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnparentObject : RewindableAction
{
    private Vector3 initialPosition;
    private Transform unparentedGameObject;
    private Transform parentGameObject;

    public static void ObjectUnparented(
        Transform unparentedGameObject,
        Transform parentGameObject,
        Vector3 initialPosition
    )
    {
        UnparentObject unparentObject = new UnparentObject(
            unparentedGameObject,
            parentGameObject,
            initialPosition
        );
    }

    public UnparentObject(
        Transform unparentedGameObject,
        Transform parentGameObject,
        Vector3 initialPosition
    )
    {
        this.unparentedGameObject = unparentedGameObject;
        this.parentGameObject = parentGameObject;
        this.initialPosition = initialPosition;
        unparentedGameObject.SetParent(null);
        Execute();
    }

    public override void Undo()
    {
        unparentedGameObject.SetParent(parentGameObject);
        unparentedGameObject.position = initialPosition;
    }
}
