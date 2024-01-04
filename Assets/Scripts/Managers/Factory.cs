using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    // public static Factory Instance { get; private set; }

    // private void Awake()
    // {
    //     if (Instance != null)
    //     {
    //         Debug.LogError("There's more than one Factory! " + transform + " - " + Instance);
    //         Destroy(gameObject);
    //         return;
    //     }
    //     Instance = this;
    // }

    public static GameObject InstantiateGameObject(
        GameObject prefab,
        Vector3 position,
        Quaternion rotation
    )
    {
        GameObject gameObject = Instantiate(prefab, position, rotation);
        ObjectInstantiation.ObjectCreated(gameObject);
        return gameObject;
    }

    public static GameObject InstantiateGameObject(GameObject prefab, Transform parent)
    {
        GameObject gameObject = Instantiate(prefab, parent);
        ObjectInstantiation.ObjectCreated(gameObject);
        return gameObject;
    }
}
