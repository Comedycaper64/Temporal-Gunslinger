using UnityEngine;

public class InspectTarget : MonoBehaviour
{
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

    private void Start()
    {
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

    public void ResetTarget()
    {
        transform.position = initialLocation;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
}
