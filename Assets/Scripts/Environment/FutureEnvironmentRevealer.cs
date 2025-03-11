using UnityEngine;

public class FutureEnvironmentRevealer : MonoBehaviour
{
    [SerializeField]
    private float reveal = 0f;

    [SerializeField]
    private FutureEnvironment futureEnvironment;

    void Start()
    {
        futureEnvironment.RevealPositionOverride(transform.position);
    }

    private void Update()
    {
        futureEnvironment.RevealRadiusOverride(reveal);
    }
}
