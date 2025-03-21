using UnityEngine;

public class TimescaleOverride : MonoBehaviour
{
    [SerializeField]
    private float timescaleOverride;

    private void Update()
    {
        if ((timescaleOverride >= 0f) && (timescaleOverride <= 1f))
        {
            Time.timeScale = timescaleOverride;
        }
    }
}
