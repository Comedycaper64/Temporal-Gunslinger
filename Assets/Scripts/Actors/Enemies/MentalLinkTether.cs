using UnityEngine;

//using Random = UnityEngine.Random;

[ExecuteAlways]
public class MentalLinkTether : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool linkEnabled = false;
    private float tau = 2 * Mathf.PI;
    private float linkOffset = 0f;
    private float linkOffsetRange = 10f;

    // [SerializeField]
    // private float randomWobbleRange = 1.1f;

    [SerializeField]
    private int points;

    [SerializeField]
    private float amplitude = 1f;

    [SerializeField]
    private float frequency = 1f;

    [SerializeField]
    private float movementSpeed = 1f;

    //[SerializeField]
    private Vector3 lineStartLocation = Vector3.zero;
    private Vector3 lineEndLocation = Vector3.zero;

    [SerializeField]
    private AnimationCurve amplitudeCurve;

    [SerializeField]
    private AnimationCurve frequencyCurve;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        linkOffset = Random.Range(0f, linkOffsetRange);
    }

    void Update()
    {
        if (linkEnabled)
        {
            Draw();
        }
    }

    private void Draw()
    {
        lineRenderer.positionCount = points;

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            Vector3 pointPosition = Vector3.Lerp(lineStartLocation, lineEndLocation, progress);

            float randomAmplitudeWobble = amplitudeCurve.Evaluate(progress);
            float randomFrequencyWobble = frequencyCurve.Evaluate(progress);

            pointPosition.y +=
                amplitude
                * randomAmplitudeWobble
                * Mathf.Sin(
                    (tau * frequency * randomFrequencyWobble * progress)
                        + (Time.timeSinceLevelLoad * movementSpeed + linkOffset)
                );
            lineRenderer.SetPosition(currentPoint, pointPosition);
        }
    }

    public void SetTetherPoint(Vector3 startPoint, Vector3 endPoint)
    {
        lineStartLocation = startPoint;
        lineEndLocation = endPoint;
        linkEnabled = true;
    }

    public void SeverTetherPoint()
    {
        lineRenderer.positionCount = 0;
        linkEnabled = false;
    }
}
