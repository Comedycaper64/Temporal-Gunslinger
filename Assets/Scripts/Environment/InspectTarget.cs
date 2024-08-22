using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectTarget : MonoBehaviour
{
    //private int initialLayer;
    private Vector3 initialLocation;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    [SerializeField]
    private string targetName;

    [SerializeField]
    [TextArea(1, 10)]
    private string targetRevenantThoughts;

    [SerializeField]
    [TextArea]
    private string targetDescription;

    [SerializeField]
    private float targetViewScale = 15f;

    // [SerializeField]
    // private List<GameObject> targetSubObjects = new List<GameObject>();

    private void Start()
    {
        //initialLayer = gameObject.layer;
        initialLocation = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    public string GetTargetName()
    {
        return targetName;
    }

    public string GetTargetThoughts()
    {
        return targetRevenantThoughts;
    }

    public string GetTargetDescription()
    {
        return targetDescription;
    }

    public float GetTargetViewScale()
    {
        return targetViewScale;
    }

    // public List<GameObject> GetTargetSubObjects()
    // {
    //     return targetSubObjects;
    // }

    public void ResetTarget()
    {
        transform.position = initialLocation;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        //gameObject.layer = initialLayer;

        // foreach (GameObject subObject in targetSubObjects)
        // {
        //     subObject.layer = initialLayer;
        // }
    }
}
