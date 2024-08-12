using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiation : RewindableAction
{
    private GameObject createdGameObject;

    public static void ObjectCreated(GameObject gameObject)
    {
        new ObjectInstantiation(gameObject);
    }

    public ObjectInstantiation(GameObject gameObject)
    {
        createdGameObject = gameObject;
        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
        Execute();
    }

    private void GameManager_OnGameStateChange(object sender, StateEnum e)
    {
        if ((e == StateEnum.idle) || (e == StateEnum.inactive))
        {
            GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
            GameObject.Destroy(createdGameObject);
        }
    }

    public override void Undo()
    {
        GameObject.Destroy(createdGameObject);
    }
}
