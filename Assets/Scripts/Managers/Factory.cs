using UnityEngine;

public class Factory : MonoBehaviour
{
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
